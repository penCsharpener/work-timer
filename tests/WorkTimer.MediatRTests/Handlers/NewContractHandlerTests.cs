using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers;
using WorkTimer.Persistence.Data;
using Xunit;

namespace WorkTimer.MediatRTests.Handlers;

public class NewContractHandlerTests
{
    private readonly NewContractHandler _testObject;
    private readonly DbContextOptions<AppDbContext> _options;

    public NewContractHandlerTests()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        _testObject = new NewContractHandler(new AppDbContext(_options), Substitute.For<ILogger<NewContractHandler>>());
    }

    [Fact]
    public async Task Handler_Creates_New_Contract()
    {
        using (var context = new AppDbContext(_options))
        {
            context.Contracts.Count().Should().Be(0);
        }

        var request = new NewContractRequest()
        {
            User = new AppUser { Id = 1 },
            Name = "New Contract",
            Employer = "Test Employer",
            HoursPerWeek = 35,
            IsCurrent = true
        };

        var response = await _testObject.Handle(request, CancellationToken.None);

        response.Should().BeTrue();

        using (var context = new AppDbContext(_options))
        {
            context.Contracts.Count().Should().Be(1);
            var contract = context.Contracts.FirstOrDefault(x => x.Name == "New Contract");
            contract.Employer.Should().Be("Test Employer");
            contract.HoursPerWeek.Should().Be(35);
            contract.IsCurrent.Should().BeTrue();
            contract.UserId.Should().Be(1);
        }
    }
}
