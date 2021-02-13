using FluentAssertions;
using System;
using System.Linq;
using WorkTimer.Domain.Extensions;
using Xunit;

namespace WorkTimer.Domain.Tests
{
    public class DateTimeExtensionTests
    {
        [Theory]
        [InlineData(2021, 02, 12, 2021, 02, 08, 2021, 02, 14)]
        public void This_DateTime_GetWholeWeek_Returns_7_Days(int testYear, int testMonth, int testDay, int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay)
        {
            var result = new DateTime(testYear, testMonth, testDay).GetWholeWeek();

            result.Count().Should().Be(7);
            result.First().Date.Should().Be(new DateTime(startYear, startMonth, startDay));
            result.Last().Date.Should().Be(new DateTime(endYear, endMonth, endDay));
        }
    }
}
