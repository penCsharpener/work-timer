using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Dev.Seeding;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR;
using WorkTimer.Persistence.Data;
using Xunit;

namespace WorkTimer.MediatRTests.Handlers
{
    public class CreateBlankWorkDayHandlerTests
    {
        private readonly CreateBlankWorkDayHandler _testObject;
        private readonly DbContextOptions<AppDbContext> _options;

        public CreateBlankWorkDayHandlerTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new AppDbContext(_options))
            {
                var contract = new Contract { Id = 1, Employer = "e", Name = "e", HoursPerWeek = 40, IsCurrent = true, UserId = 1 };
                var user = new AppUser { Id = 1, Contracts = new List<Contract> { contract } };
                context.Contracts.Add(contract);
                context.Users.Add(user);
                context.SaveChanges();

                context.WorkDays.AddRange(new WorkDaySeeder(contract).AddWorkDayRange("2021-01-01", "2021-02-19").Seed());
                context.SaveChanges();
            }

            _testObject = new CreateBlankWorkDayHandler(new AppDbContext(_options), Substitute.For<ILogger<CreateBlankWorkDayHandler>>());
        }

        [Fact]
        public async Task Creates_New_Workday()
        {
            using (var context = new AppDbContext(_options))
            {
                var user = await context.Users.Include(x => x.Contracts).FirstOrDefaultAsync(x => x.Id == 1);
                var response = await _testObject.Handle(new CreateBlankWorkDayCommand(new DateTime(2021, 02, 27)) { User = user }, CancellationToken.None);

                response.WorkDay.ContractId.Should().Be(1);
                response.WorkDay.Date.Should().Be(new DateTime(2021, 02, 27));
                response.WorkDay.RequiredHours.Should().Be(8);
                response.WorkDay.TotalHours.Should().Be(0);
                response.WorkDay.WorkingPeriods.Should().BeNull();
                response.WorkDay.WorkDayType.Should().Be(WorkDayType.Weekend);
            }
        }

        [Fact]
        public async Task Creates_No_Workday_When_Exists()
        {
            using (var context = new AppDbContext(_options))
            {
                var user = await context.Users.Include(x => x.Contracts).FirstOrDefaultAsync(x => x.Id == 1);

                var response = await _testObject.Handle(new CreateBlankWorkDayCommand(new DateTime(2021, 02, 2)) { User = user }, CancellationToken.None);

                response.WorkDay.ContractId.Should().Be(1);
                response.WorkDay.Date.Should().Be(new DateTime(2021, 02, 2));
                response.WorkDay.RequiredHours.Should().Be(8);
                response.WorkDay.TotalHours.Should().Be(7.2833333333333332);
                response.WorkDay.WorkingPeriods.Should().BeNull();
                response.WorkDay.WorkDayType.Should().Be(WorkDayType.Workday);
            }
        }

        [Fact]
        public async Task Creates_No_Workday_With_VacationDay()
        {
            using (var context = new AppDbContext(_options))
            {
                var user = await context.Users.Include(x => x.Contracts).FirstOrDefaultAsync(x => x.Id == 1);

                var response = await _testObject.Handle(new CreateBlankWorkDayCommand(new DateTime(2021, 02, 26)) { WorkDayType = WorkDayType.Vacation, User = user }, CancellationToken.None);

                response.WorkDay.ContractId.Should().Be(1);
                response.WorkDay.Date.Should().Be(new DateTime(2021, 02, 26));
                response.WorkDay.RequiredHours.Should().Be(8);
                response.WorkDay.TotalHours.Should().Be(0);
                response.WorkDay.WorkingPeriods.Should().BeNull();
                response.WorkDay.WorkDayType.Should().Be(WorkDayType.Vacation);
            }
        }
    }
}
