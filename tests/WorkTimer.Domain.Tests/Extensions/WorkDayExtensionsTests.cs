using FluentAssertions;
using System;
using System.Collections.Generic;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using Xunit;

namespace WorkTimer.Domain.Tests.Extensions
{
    public class WorkDayExtensionsTests
    {
        private readonly WorkDay _testObject;

        public WorkDayExtensionsTests()
        {
            _testObject = new WorkDay
            {
                Contract = new Contract { HoursPerWeek = 40 },
                TotalHours = 6,
                WorkDayType = WorkDayType.Workday,
                WorkingPeriods = new List<WorkingPeriod>
                {
                    new WorkingPeriod { StartTime = new DateTime(2021, 2, 4, 9, 0, 0), EndTime = new DateTime(2021, 2, 4, 12, 0, 0) },
                    new WorkingPeriod { StartTime = new DateTime(2021, 2, 4, 14, 0, 0), EndTime = new DateTime(2021, 2, 4, 16, 0, 0) },
                    new WorkingPeriod { StartTime = new DateTime(2021, 2, 4, 17, 0, 0), EndTime = new DateTime(2021, 2, 4, 20, 0, 0) },
                }
            };
        }

        [Fact]
        public void GetContractedHoursPerDay_Returns_Hours_Per_Day()
        {
            _testObject.Contract.GetContractedHoursPerDay().Should().Be(8);
        }

        [Fact]
        public void GetContractedHoursPerDay_Returns_Zero_For_Missing_Contract()
        {
            _testObject.Contract = null;
            _testObject.Contract.GetContractedHoursPerDay().Should().Be(0);
        }

        [Fact]
        public void GetWorkTime_Gets_TimeSpan_Of_TotalHours()
        {
            _testObject.GetWorkTime().Should().Be(new System.TimeSpan(0, 6, 0, 0, 0));
        }

        [Theory]
        [InlineData(WorkDayType.HalfVacation, 0.5)]
        [InlineData(WorkDayType.Workday, 1)]
        [InlineData(WorkDayType.Undefined, 1)]
        [InlineData(WorkDayType.SickDay, 0)]
        [InlineData(WorkDayType.ChildSickDay, 0)]
        [InlineData(WorkDayType.BankHoliday, 0)]
        [InlineData(WorkDayType.Vacation, 0)]
        [InlineData(WorkDayType.ParentalLeave, 0)]
        [InlineData(WorkDayType.Weekend, 0)]
        public void GetWorkHourMultiplier_Gets_Multiplier(WorkDayType workDayType, double expectedResult)
        {
            _testObject.WorkDayType = workDayType;
            _testObject.GetWorkHourMultiplier().Should().Be(expectedResult);
            _testObject.WorkDayType.GetWorkHourMultiplier().Should().Be(expectedResult);
        }

        [Fact]
        public void CalculateTotalHours_Returns_TotalHours()
        {
            _testObject.CalculateTotalHours().TotalHours.Should().Be(8);
        }

        [Fact]
        public void CalculateTotalHours_Skips_Calculation()
        {
            _testObject.WorkingPeriods = null;
            _testObject.CalculateTotalHours().TotalHours.Should().Be(6);
        }
    }
}
