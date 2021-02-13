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
using WorkTimer.MediatR.Services.Abstractions;
using WorkTimer.Persistence.Data;
using Xunit;

namespace WorkTimer.MediatRTests.Handlers
{
    public class NewWorkPeriodHandlerTests
    {
        private readonly NewWorkingPeriodHandler _testObject;
        private readonly DbContextOptions<AppDbContext> _options;
        private static MockNow _now = new MockNow();

        public NewWorkPeriodHandlerTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new AppDbContext(_options))
            {
                context.Contracts.Add(new Contract { Id = 1, Employer = "e", Name = "e", HoursPerWeek = 40, IsCurrent = true, UserId = 1 });
                context.SaveChanges();
            }

            _testObject = new NewWorkingPeriodHandler(new AppDbContext(_options), _now, Substitute.For<ILogger<NewWorkingPeriodHandler>>());
        }

        [Fact]
        public async Task Handler_Adds_Workday_And_WorkPeriod()
        {
            _now.SetDateTime(DateTime.Now);

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Count().Should().Be(0);
                context.WorkingPeriods.Count().Should().Be(0);
            }

            var request = new NewWorkingPeriodRequest()
            {
                User = new AppUser { Id = 1 },
                Comment = "Test Comment"
            };

            var response = await _testObject.Handle(request, CancellationToken.None);
            response.Should().BeTrue();

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Count().Should().Be(1);
                context.WorkingPeriods.Count().Should().Be(1);
            }
        }

        [Fact]
        public async Task Handler_Adds_WorkPeriod_To_Existing_Workday()
        {
            _now.SetDateTime(new DateTime(2021, 2, 10, 19, 0, 0));

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Add(new WorkDay { ContractId = 1, TotalHours = 0, WorkDayType = WorkDayType.Workday, Date = _now.Now.Date });
                context.WorkingPeriods.Count().Should().Be(0);
                context.SaveChanges();
            }

            var request = new NewWorkingPeriodRequest()
            {
                User = new AppUser { Id = 1 },
                Comment = "Test Comment"
            };

            var response = await _testObject.Handle(request, CancellationToken.None);
            response.Should().BeTrue();

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Count().Should().Be(1);
                context.WorkingPeriods.Count().Should().Be(1);
            }
        }

        [Fact]
        public async Task Handler_Ends_WorkPeriod()
        {
            _now.SetDateTime(new DateTime(2021, 2, 10, 19, 0, 0));

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Add(new WorkDay { Id = 1, ContractId = 1, TotalHours = 0, WorkDayType = WorkDayType.Workday, Date = _now.Now.Date });
                context.WorkingPeriods.Add(new WorkingPeriod { WorkDayId = 1, StartTime = new DateTime(2021, 2, 10, 9, 0, 0), EndTime = new DateTime(2021, 2, 10, 12, 30, 0) });
                context.WorkingPeriods.Add(new WorkingPeriod { WorkDayId = 1, StartTime = new DateTime(2021, 2, 10, 14, 0, 0) });
                context.SaveChanges();
            }

            var request = new NewWorkingPeriodRequest()
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
                lastWorkPeriod.EndTime.Value.Date.Should().Be(_now.Now.Date);
            }
        }

        [Fact]
        public async Task Handler_Completes_WorkPeriod_Even_On_New_Workday()
        {
            _now.SetDateTime(new DateTime(2021, 2, 12, 15, 0, 0));

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Add(new WorkDay { Id = 1, ContractId = 1, TotalHours = 0, WorkDayType = WorkDayType.Workday, Date = new DateTime(2021, 2, 10) });
                context.WorkingPeriods.Add(new WorkingPeriod { WorkDayId = 1, StartTime = new DateTime(2021, 2, 10, 14, 0, 0) });
                context.SaveChanges();
            }

            var request = new NewWorkingPeriodRequest()
            {
                User = new AppUser { Id = 1 },
                Comment = "Test Comment"
            };

            var response = await _testObject.Handle(request, CancellationToken.None);

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Count().Should().Be(1);
                context.WorkingPeriods.Count().Should().Be(1);
                context.WorkingPeriods.Count(x => !x.EndTime.HasValue).Should().Be(0);
            }
        }

        public class MockNow : INow
        {
            private DateTime _now;

            public void SetDateTime(DateTime now)
            {
                _now = now;
            }

            public DateTime Now => _now;
        }
    }
}
