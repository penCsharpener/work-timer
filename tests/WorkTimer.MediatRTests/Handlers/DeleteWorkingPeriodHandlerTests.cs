using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers;
using WorkTimer.MediatR.Requests;
using WorkTimer.Messaging.Abstractions;
using WorkTimer.Persistence.Data;
using Xunit;

namespace WorkTimer.MediatRTests.Handlers
{
    public class DeleteWorkingPeriodHandlerTests
    {
        private readonly DeleteWorkingPeriodHandler _testObject;
        private readonly DbContextOptions<AppDbContext> _options;
        private readonly IMessageService _messageService;

        public DeleteWorkingPeriodHandlerTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new AppDbContext(_options))
            {
                context.Contracts.Add(new Contract { Id = 1, Employer = "e", Name = "e", HoursPerWeek = 40, IsCurrent = true, UserId = 1 });
                context.WorkDays.Add(new WorkDay
                {
                    Id = 1,
                    ContractId = 1,
                    Date = new DateTime(2021, 2, 1),
                    WorkDayType = WorkDayType.Workday,
                    TotalHours = 8,
                    WorkingPeriods = new List<WorkingPeriod>
                    {
                        new WorkingPeriod { Id = 1, StartTime = new DateTime(2021, 2,1,8,0,0), EndTime = new DateTime(2021, 2,1,11,0,0) },
                        new WorkingPeriod { Id = 2, StartTime = new DateTime(2021, 2,1,12,0,0), EndTime = new DateTime(2021, 2,1,14,0,0) },
                        new WorkingPeriod { Id = 3, StartTime = new DateTime(2021, 2,1,15,0,0), EndTime = new DateTime(2021, 2,1,18,0,0) }
                    }
                });
                context.SaveChanges();
            }

            _messageService = Substitute.For<IMessageService>();
            _testObject = new DeleteWorkingPeriodHandler(new AppDbContext(_options), _messageService, Substitute.For<ILogger<DeleteWorkingPeriodHandler>>());
        }

        [Fact]
        public async Task Handler_Removes_WorkPeriod_And_Updates_TotalHours()
        {
            WorkingPeriod workPeriod;

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.FirstOrDefault().TotalHours.Should().Be(8);
                context.WorkingPeriods.Count().Should().Be(3);
                workPeriod = context.WorkingPeriods.Include(x => x.WorkDay).FirstOrDefault(x => x.Id == 3);
            }

            var request = new DeleteWorkingPeriodRequest(workPeriod)
            {
                User = new AppUser { Id = 1 },
            };

            var response = await _testObject.Handle(request, CancellationToken.None);
            response.Should().BeTrue();

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Count().Should().Be(1);
                context.WorkDays.FirstOrDefault().TotalHours.Should().Be(5);
                context.WorkingPeriods.Count().Should().Be(2);
            }
        }

        [Fact]
        public async Task Handler_Returns_False_On_Empty_Request()
        {
            var response = await _testObject.Handle(new DeleteWorkingPeriodRequest(null), CancellationToken.None);

            response.Should().BeFalse();

            response = await _testObject.Handle(default(DeleteWorkingPeriodRequest), CancellationToken.None);

            response.Should().BeFalse();
        }
    }
}
