using EasyNetQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Handlers;
using WorkTimer.Messaging;
using WorkTimer.Messaging.MessageModels;
using WorkTimer.Persistence.Data;
using Xunit;

namespace WorkTimer.Integration;

public class RecalculateHoursOfUserTests
{
    private readonly MessageWorker _worker;
    private readonly DbContextOptions<AppDbContext> _options;
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;
    private readonly DeleteWorkingPeriodHandler _handler;

    public RecalculateHoursOfUserTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        using (var context = new AppDbContext(_options))
        {

        }

        _bus = Substitute.For<IBus>();
        _serviceProvider = new ServiceCollection().AddScoped(_ => new AppDbContext(_options)).BuildServiceProvider();

        _worker = new MessageWorker(_bus, _serviceProvider, Substitute.For<ILogger<MessageWorker>>());
    }

    [Fact]
    public async Task Index_Runs_RecalculateHoursOfUserAsync()
    {
        var handlerRequest = new DeleteWorkingPeriodRequest(new() { });

        await _handler.Handle(handlerRequest, CancellationToken.None);

        var request = new UpdateTotalHoursFromWorkDayMessage(4);
        await _worker.UpdateTotalHoursFromWorkDayAsync(request, CancellationToken.None);
    }
}
