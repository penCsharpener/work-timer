using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers.Stats;
using WorkTimer.Persistence.Data;
using Xunit;

namespace WorkTimer.MediatRTests.Handlers
{
    public class RecalculateMyWeeksHandlerTests
    {
        private readonly RecalculateMyWeeksHandler _testObject;
        private readonly DbContextOptions<AppDbContext> _options;

        public RecalculateMyWeeksHandlerTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new AppDbContext(_options))
            {
                context.Contracts.Add(new Contract { Id = 1, Employer = "e", Name = "e", HoursPerWeek = 40, IsCurrent = true, UserId = 1 });
                context.WorkWeeks.Add(new WorkWeek { Id = 1, WeekStart = new DateTime(2021, 2, 8), WeekNumber = 6 });
                context.SaveChanges();
            }

            _testObject = new RecalculateMyWeeksHandler(new AppDbContext(_options), Substitute.For<ILogger<RecalculateMyWeeksHandler>>());
        }

        [Fact]
        public async Task Handler_Finds_Workdays()
        {
            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.Add(new WorkDay { Id = 1, Date = new DateTime(2021, 2, 3), ContractId = 1, WorkDayType = WorkDayType.Workday, TotalHours = 1 });
                context.WorkDays.Add(new WorkDay { Id = 2, Date = new DateTime(2021, 2, 4), ContractId = 1, WorkDayType = WorkDayType.Workday, TotalHours = 2 });
                context.WorkDays.Add(new WorkDay { Id = 3, Date = new DateTime(2021, 2, 5), ContractId = 1, WorkDayType = WorkDayType.Workday, TotalHours = 3 });
                context.WorkDays.Add(new WorkDay { Id = 4, Date = new DateTime(2021, 2, 9), ContractId = 1, WorkDayType = WorkDayType.Workday, TotalHours = 4 });
                context.SaveChanges();
            }

            var response = await _testObject.Handle(new RecalculateMyWeeksRequest(2021, 5) { User = new AppUser { Id = 1 } }, CancellationToken.None);
        }
    }
}
