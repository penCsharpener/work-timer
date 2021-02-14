using FluentAssertions;
using System;
using System.Linq;
using WorkTimer.Dev.Seeding;
using Xunit;

namespace WorkTimer.Seeding.Tests
{
    public class WorkingPeriodSeederTests
    {
        private readonly WorkingPeriodSeeder _testObject;

        public WorkingPeriodSeederTests()
        {
            _testObject = new WorkingPeriodSeeder(1, new DateTime(2021, 2, 5), 3);
        }

        [Fact]
        public void WorkDaySeeder_Creates_WorkDays_And_Periods()
        {
            var list = _testObject.Seed();
            list.Count.Should().Be(3);
            var expectedDate = new DateTime(2021, 2, 5);
            list.All(x => x.StartTime.Date == expectedDate && x.EndTime.Value.Date == expectedDate);
            list[0].StartTime.Should().Be(new DateTime(2021, 2, 5, 8, 1, 0));
            list[2].EndTime.Value.Should().Be(new DateTime(2021, 2, 5, 18, 26, 0));
        }
    }
}
