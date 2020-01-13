using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Blazor.Extensions;
using WorkTimer.Contracts;
using WorkTimer.Models;
using WorkTimer.Services;

namespace WorkTimer.Tests {

    [TestFixture]
    public class TrackingTests {

        private IServiceProvider _serviceProvider;

        public TrackingTests() {
            var services = new ServiceCollection();
            services.WireUpMockClasses();

            _serviceProvider = services.BuildServiceProvider();
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task StartTrackingChecks(bool expectedIncomplete) {
            var trackingBase = new StartTrackingBase(_serviceProvider.GetRequiredService<IWorkingDayRepository>(),
                                                     _serviceProvider.GetRequiredService<IWorkPeriodRepository>(),
                                                     _serviceProvider.GetRequiredService<IWorkBreakRepository>(),
                                                     _serviceProvider.GetRequiredService<IWriterWorkingDay>(),
                                                     _serviceProvider.GetRequiredService<IWriterWorkPeriod>(),
                                                     _serviceProvider.GetRequiredService<IWriterWorkBreak>());
            if (expectedIncomplete) {
                var lastestMockDate = Repositories.MockWorkingDayRepository.Data.LastOrDefault();
                var newItem = new WorkPeriod() {
                    WorkingDayId = lastestMockDate.Id,
                    StartTime = lastestMockDate.Date.AddHours(6).AddMinutes(30).AddSeconds(0),
                    Id = 11
                };
                await _serviceProvider.GetService<IWriterWorkPeriod>().Insert(newItem);
                Assert.IsTrue(await trackingBase.IncompletePeriodExists());
                await _serviceProvider.GetService<IWriterWorkPeriod>().Delete(newItem);
            } else {
                Assert.IsFalse(await trackingBase.IncompletePeriodExists());
            }
        }

        [TestCase()]
        public async Task ToggleTrackingAsyncTest() {
            var tracker = _serviceProvider.GetService<IToggleTracking>();
            await tracker.ToggleTracking(DateTime.Now);
            await Task.Delay(5);
            await tracker.ToggleTracking(DateTime.Now);
            Assert.IsTrue(true);
        }
    }
    /*
        public class TrackingTests2 {

            [SetUp]
            public void Setup() {

            }

            [TestCase()]
            public void TestTemplate() {

                Assert.IsTrue(true);
            }

            [TestCase()]
            public async Task TestTemplateAsync() {
                await Task.Delay(0);
                Assert.IsTrue(true);
            }
    */
}
