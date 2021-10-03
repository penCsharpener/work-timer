using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Dev.Seeding;
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
                context.Users.Add(new AppUser { Id = 1 });
                context.Contracts.Add(new Contract { Id = 1, Employer = "e", Name = "e", HoursPerWeek = 40, IsCurrent = true, UserId = 1 });
                context.WorkWeeks.Add(new WorkWeek { Id = 1, ContractId = 1, WeekStart = new DateTime(2021, 2, 1), WeekNumber = 5 });
                context.WorkWeeks.Add(new WorkWeek { Id = 2, ContractId = 1, WeekStart = new DateTime(2021, 2, 8), WeekNumber = 6 });
                context.SaveChanges();
            }

            _testObject = new RecalculateMyWeeksHandler(new AppDbContext(_options), Substitute.For<ILogger<RecalculateMyWeeksHandler>>());
        }

        [Fact]
        public async Task Handler_Calculates_Stats_For_Specific_Week()
        {
            using (var context = new AppDbContext(_options))
            {
                AddWorkDays(context);
                context.SaveChanges();

                var testWeek = await context.WorkWeeks.FirstOrDefaultAsync(x => x.Id == 1);
                testWeek.TotalHours.Should().Be(0);
                testWeek.TotalOverhours.Should().Be(0);
            }

            var response = await _testObject.Handle(new RecalculateMyWeeksRequest(2021, 5) { User = new AppUser { Id = 1 }, CurrentContract = new() { Id = 1 } }, CancellationToken.None);

            using (var context = new AppDbContext(_options))
            {
                var testWeek = await context.WorkWeeks.FirstOrDefaultAsync(x => x.Id == 1);
                testWeek.TotalHours.Should().Be(18);
                testWeek.TotalOverhours.Should().Be(-22);
                testWeek.DaysOffWork.Should().Be(2);
                testWeek.DaysWorked.Should().Be(5);
            }
        }

        [Fact]
        public async Task Handler_Calculates_Stats_For_All_Weeks()
        {
            AppUser user = null;

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.AddRange(new WorkDaySeeder(new Contract { Id = 1, HoursPerWeek = 40 }).AddWorkDayRange("2020-01-01", "2021-02-14").Seed());
                context.SaveChanges();

                var testWeek = await context.WorkWeeks.FirstOrDefaultAsync(x => x.Id == 1);
                testWeek.TotalHours.Should().Be(0);
                testWeek.TotalOverhours.Should().Be(0);
                user = await context.Users.Include(x => x.Contracts.Where(c => c.IsCurrent)).FirstOrDefaultAsync(x => x.Id == 1);
            }

            var response = await _testObject.Handle(new RecalculateMyWeeksRequest() { User = user, CurrentContract = new() { Id = 1 } }, CancellationToken.None);

            using (var context = new AppDbContext(_options))
            {
                var weeks = await context.WorkWeeks.ToListAsync();
                weeks.Count.Should().Be(59);
                var daysAssigned = await context.WorkDays.Where(x => x.WorkWeekId.HasValue).ToListAsync();
                daysAssigned.Count.Should().Be(284);
            }
        }

        private void AddWorkDays(AppDbContext context)
        {
            context.WorkDays.Add(new WorkDay { Id = 1, RequiredHours = 8, Date = new DateTime(2021, 2, 1), ContractId = 1, WorkDayType = WorkDayType.Workday, TotalHours = 1 });
            context.WorkDays.Add(new WorkDay { Id = 2, RequiredHours = 8, Date = new DateTime(2021, 2, 2), ContractId = 1, WorkDayType = WorkDayType.Workday, TotalHours = 2 });
            context.WorkDays.Add(new WorkDay { Id = 3, RequiredHours = 8, Date = new DateTime(2021, 2, 3), ContractId = 1, WorkDayType = WorkDayType.Workday, TotalHours = 3 });
            context.WorkDays.Add(new WorkDay { Id = 4, RequiredHours = 8, Date = new DateTime(2021, 2, 4), ContractId = 1, WorkDayType = WorkDayType.Workday, TotalHours = 3 });
            context.WorkDays.Add(new WorkDay { Id = 5, RequiredHours = 8, Date = new DateTime(2021, 2, 5), ContractId = 1, WorkDayType = WorkDayType.Workday, TotalHours = 3 });
            context.WorkDays.Add(new WorkDay { Id = 6, RequiredHours = 0, Date = new DateTime(2021, 2, 6), ContractId = 1, WorkDayType = WorkDayType.Weekend, TotalHours = 3 });
            context.WorkDays.Add(new WorkDay { Id = 7, RequiredHours = 0, Date = new DateTime(2021, 2, 7), ContractId = 1, WorkDayType = WorkDayType.Weekend, TotalHours = 3 });
            context.WorkDays.Add(new WorkDay { Id = 8, RequiredHours = 8, Date = new DateTime(2021, 2, 8), ContractId = 1, WorkDayType = WorkDayType.Workday, TotalHours = 4 });
        }
    }
}
