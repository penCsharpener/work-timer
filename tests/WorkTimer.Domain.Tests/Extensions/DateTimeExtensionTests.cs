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

        [Theory]
        [InlineData(2021, 1, 1, 52)]
        [InlineData(2021, 2, 2, 5)]
        [InlineData(2021, 2, 7, 5)]
        [InlineData(2021, 2, 25, 8)]
        [InlineData(2021, 12, 31, 52)]
        public void This_DateTime_GetWeekNumber_Returns_WeekNumber(int testYear, int testMonth, int testDay, int expectedWeekNumber)
        {
            new DateTime(testYear, testMonth, testDay).GetWeekNumber().Should().Be(expectedWeekNumber);
        }

        [Theory]
        [InlineData(2021, 1, 1, 8)]
        [InlineData(2021, 5, 2, 5)]
        [InlineData(2021, 8, 2, 26)]
        [InlineData(2021, 40, 10, 8)]
        [InlineData(2021, 52, 1, 1)]
        public void This_DateTime_GetWeekNumber_Returns_WeekNumber_And_Date(int year, int expectedWeekNumber, int testMonth, int testDay)
        {
            DateTimeExtensions.GetWeekNumber(year, expectedWeekNumber, out var date);
            date.Should().Be(new DateTime(year, testMonth, testDay));
        }
    }
}
