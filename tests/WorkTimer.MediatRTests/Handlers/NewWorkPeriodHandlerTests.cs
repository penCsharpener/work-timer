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
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;
using Xunit;

namespace WorkTimer.MediatRTests.Handlers
{
    public class NewWorkPeriodHandlerTests
    {
        private readonly NewWorkPeriodHandler _testObject;
        private readonly DbContextOptions<AppDbContext> _options;

        public NewWorkPeriodHandlerTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new AppDbContext(_options))
            {
                context.Contracts.Add(new Contract { Employer = "e", Name = "e", HoursPerWeek = 40, IsCurrent = true, UserId = 1 });
                context.SaveChanges();
            }

            _testObject = new NewWorkPeriodHandler(new AppDbContext(_options), Substitute.For<ILogger<NewWorkPeriodHandler>>());
        }

        [Fact]
        public async Task Handler_Adds_Workday_And_WorkPeriod()
        {
            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Count().Should().Be(0);
                context.WorkingPeriods.Count().Should().Be(0);
            }

            var request = new NewWorkPeriodRequest()
            {
                User = new AppUser { Id = 1 },
                Comment = "Test Comment"
            };

            var response = await _testObject.Handle(request, CancellationToken.None);

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Count().Should().Be(1);
                context.WorkingPeriods.Count().Should().Be(1);
            }
        }

        [Fact]
        public async Task Handler_Adds_WorkPeriod_To_Existing_Workday()
        {
            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Add(new WorkDay { ContractId = 1, TotalHours = 0, WorkDayType = WorkDayType.Workday, Date = DateTime.Today });
                context.WorkingPeriods.Count().Should().Be(0);
                context.SaveChanges();
            }

            var request = new NewWorkPeriodRequest()
            {
                User = new AppUser { Id = 1 },
                Comment = "Test Comment"
            };

            var response = await _testObject.Handle(request, CancellationToken.None);

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Count().Should().Be(1);
                context.WorkingPeriods.Count().Should().Be(1);
            }
        }

        [Fact]
        public async Task Handler_Ends_WorkPeriod()
        {
            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Add(new WorkDay { Id = 1, ContractId = 1, TotalHours = 0, WorkDayType = WorkDayType.Workday, Date = DateTime.Today });
                context.WorkingPeriods.Add(new WorkingPeriod { WorkDayId = 1, StartTime = new DateTime(2021, 2, 10, 9, 0, 0), EndTime = new DateTime(2021, 2, 10, 12, 30, 0) });
                context.WorkingPeriods.Add(new WorkingPeriod { WorkDayId = 1, StartTime = new DateTime(2021, 2, 10, 14, 0, 0) });
                context.SaveChanges();
            }

            var request = new NewWorkPeriodRequest()
            {
                User = new AppUser { Id = 1 },
                Comment = "Test Comment"
            };

            var response = await _testObject.Handle(request, CancellationToken.None);

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Count().Should().Be(1);
                context.WorkingPeriods.Count().Should().Be(2);
                var lastWorkPeriod = context.WorkingPeriods.FirstOrDefault(x => x.Id == 2);
                lastWorkPeriod.EndTime.Value.Date.Should().Be(DateTime.Today);
            }
        }
    }
}
