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
            _testObject = new WorkDaySeeder(1);
        }

        [Fact]
        public void WorkDaySeeder_Creates_WorkDays_And_Periods()
        {
            _testObject.AddWorkDayRange(DateTime.Today.AddDays(-7), DateTime.Today);

            var list = _testObject.Seed();
            list.Count.Should().Be(5);
            list.Sum(x => x.WorkingPeriods.Count).Should().Be(13);
            var totalHours = list.Sum(x => x.TotalHours);
            totalHours.Should().Be(43.949999999999996);
        }
    }
}
