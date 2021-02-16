using FluentAssertions;
using System;
using System.Linq;
using WorkTimer.Dev.Seeding;
using Xunit;

namespace WorkTimer.Seeding.Tests
{
    public class WorkDaySeederTests
    {
        private readonly WorkDaySeeder _testObject;

        public WorkDaySeederTests()
        {
            _testObject = new WorkDaySeeder(new Domain.Models.Contract { Id = 1, HoursPerWeek = 40 });
        }

        [Fact]
        public void WorkDaySeeder_Creates_WorkDays_And_Periods()
        {
            var date = new DateTime(2021, 2, 12);
            _testObject.AddWorkDayRange(date.AddDays(-6), date);

            var list = _testObject.Seed();
            list.Count.Should().Be(5);
            list.Sum(x => x.WorkingPeriods.Count).Should().Be(13);
            var totalHours = list.Sum(x => x.TotalHours);
            totalHours.Should().Be(46.11666666666666);
        }
    }
}
