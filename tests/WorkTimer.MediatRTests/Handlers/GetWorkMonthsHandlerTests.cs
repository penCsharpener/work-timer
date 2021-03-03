using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Dev.Seeding;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers;
using WorkTimer.MediatR.Handlers.Stats;
using WorkTimer.Persistence.Data;
using Xunit;

namespace WorkTimer.MediatRTests.Handlers
{
    public class GetWorkMonthsHandlerTests
    {
        private readonly GetWorkMonthsHandler _testObject;
        private readonly DbContextOptions<AppDbContext> _options;

        public GetWorkMonthsHandlerTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new AppDbContext(_options))
            {
                context.Users.Add(new AppUser { Id = 1 });
                context.Contracts.Add(new Contract { Id = 1, Employer = "e", Name = "e", HoursPerWeek = 40, IsCurrent = true, UserId = 1 });
                context.Contracts.Add(new Contract { Id = 2, Employer = "a", Name = "a", HoursPerWeek = 0, IsCurrent = false, UserId = 1 });
                context.WorkDays.AddRange(new WorkDaySeeder(new Contract { Id = 1, HoursPerWeek = 40, UserId = 1 }).AddWorkDayRange("2020-01-01", "2021-03-01").Seed());
                context.WorkDays.AddRange(new WorkDaySeeder(new Contract { Id = 2, HoursPerWeek = 40, UserId = 1 }).AddWorkDayRange("2021-01-01", "2021-03-01").Seed());
                context.SaveChanges();
            }

            _testObject = new GetWorkMonthsHandler(new AppDbContext(_options), new LoggerMock<GetWorkMonthsHandler>());
        }

        [Fact]
        public async Task Handler_Gets_Monthly_Stats()
        {
            var currentContract = new Contract { Id = 1, IsCurrent = true, UserId = 1 };
            var user = new AppUser { Id = 1, Contracts = new List<Contract>() { currentContract } };

            var workMonthCalculation = new RecalculateMyMonthsHandler(new AppDbContext(_options), new LoggerMock<RecalculateMyMonthsHandler>());
            await workMonthCalculation.Handle(new RecalculateMyMonthsRequest { User = user, CurrentContract = currentContract }, CancellationToken.None);

            using (var context = new AppDbContext(_options))
            {
                var workdays = await context.WorkDays.Include(x => x.Contract).ToListAsync();
            }

            var request = new GetWorkMonthsRequest() { User = user, CurrentContract = currentContract };

            var response = await _testObject.Handle(request, CancellationToken.None);

            response.Months.Count.Should().Be(15);
            response.Months.Sum(x => x.TotalHours).Should().Be(2656.3);
        }
    }
}
