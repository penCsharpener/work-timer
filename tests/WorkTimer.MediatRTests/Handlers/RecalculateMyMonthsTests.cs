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
    public class RecalculateMyMonthsTests
    {
        private readonly RecalculateMyMonthsHandler _testObject;
        private readonly DbContextOptions<AppDbContext> _options;

        public RecalculateMyMonthsTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new AppDbContext(_options))
            {
                context.Users.Add(new AppUser { Id = 1 });
                context.Contracts.Add(new Contract { Id = 1, Employer = "e", Name = "e", HoursPerWeek = 40, IsCurrent = true, UserId = 1 });
                context.WorkMonths.Add(new WorkMonth { Id = 1, Year = 2021, Month = 2, UserId = 1 });
                context.SaveChanges();
            }

            _testObject = new RecalculateMyMonthsHandler(new AppDbContext(_options), Substitute.For<ILogger<RecalculateMyMonthsHandler>>());
        }

        [Fact]
        public async Task Handler_Calculates_Stats_For_All_Months()
        {
            AppUser user = null;

            using (var context = new AppDbContext(_options))
            {
                context.WorkDays.AddRange(new WorkDaySeeder(new Contract { Id = 1, HoursPerWeek = 40 }).AddWorkDayRange("2020-01-01", "2021-02-14").Seed());
                context.SaveChanges();

                user = await context.Users.Include(x => x.Contracts.Where(c => c.IsCurrent)).FirstOrDefaultAsync(x => x.Id == 1);
            }

            var response = await _testObject.Handle(new RecalculateMyMonthsRequest() { User = user }, CancellationToken.None);

            using (var context = new AppDbContext(_options))
            {
                var months = await context.WorkMonths.ToListAsync();
                months.Count.Should().Be(14);
                var daysAssigned = await context.WorkDays.Where(x => x.WorkMonthId.HasValue).ToListAsync();
                daysAssigned.Count.Should().Be(284);
            }
        }
    }
}
