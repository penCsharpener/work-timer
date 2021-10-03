using FluentAssertions;
using System;
using System.Linq;
using WorkTimer.MediatR.Handlers.Stats;
using Xunit;

namespace WorkTimer.MediatRTests.Requests
{
    public class RecalculateMyWeeksRequestTests
    {
        [Fact]
        public void Request_Finds_Week_From_Week_Number()
        {
            var request = new RecalculateMyWeeksRequest(2021, 5);

            request.CalendarWeek.Should().Be(5);
            request.DaysInWeek.First().Should().Be(new DateTime(2021, 2, 1));
            request.DaysInWeek.Last().Should().Be(new DateTime(2021, 2, 7));
        }

        [Fact]
        public void Request_Finds_Week_From_Date()
        {
            var request = new RecalculateMyWeeksRequest(new DateTime(2021, 2, 5));

            request.CalendarWeek.Should().Be(5);
            request.DaysInWeek.First().Should().Be(new DateTime(2021, 2, 1));
            request.DaysInWeek.Last().Should().Be(new DateTime(2021, 2, 7));
        }
    }
}
