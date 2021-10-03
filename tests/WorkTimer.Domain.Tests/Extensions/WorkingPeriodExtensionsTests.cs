using FluentAssertions;
using System;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using Xunit;

namespace WorkTimer.Domain.Tests.Extensions
{
    public class WorkingPeriodExtensionsTests
    {
        [Fact]
        public void GetWorkTime_Gets_TimeSpan()
        {
            var wp = new WorkingPeriod() { EndTime = new DateTime(2021, 2, 4, 10, 0, 0), StartTime = new DateTime(2021, 2, 4, 9, 1, 0) };

            wp.GetWorkTime().Should().Be(TimeSpan.FromMinutes(59));
        }

        [Fact]
        public void GetWorkTime_Gets_TimeSpan_Without_EndTime()
        {
            var wp = new WorkingPeriod() { StartTime = DateTime.Now.AddMinutes(-59) };

            var minutes = Convert.ToInt32(wp.GetWorkTime().TotalMinutes);
            minutes.Should().Be(59);
        }
    }
}
