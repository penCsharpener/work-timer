using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Blazor.Extensions;
using WorkTimer.Contracts;
using WorkTimer.Models;
using WorkTimer.Repositories;

namespace WorkTimer.Tests {

    [TestFixture]
    public class RepoTests {

        private IServiceProvider _serviceProvider;

        public RepoTests() {
            var services = new ServiceCollection();
            services.WireUpMockClasses();

            _serviceProvider = services.BuildServiceProvider();
        }
        

        [Test]
        public async Task Repo_WorkingDay_GetAll_Test() {

            var repo = _serviceProvider.GetService<IWorkingDayRepository>();
            var items = await repo.GetAll();

            Assert.IsTrue(items.Any());
        }

        [Test]
        public async Task Repo_WorkPeriod_GetAll_Test() {

            var repo = _serviceProvider.GetService<IWorkPeriodRepository>();
            var items = await repo.GetAll();

            Assert.IsTrue(items.Any());
        }

        [TestCase(6, 33, 1)]
        public async Task Repo_WorkingDay_GetIncomplete_Test(int hour, int min, int sec) {

            var repo = _serviceProvider.GetService<IWorkingDayRepository>();
            var lastestMockDate = MockWorkingDayRepository.Data.LastOrDefault();
            await _serviceProvider.GetService<IWriterWorkPeriod>().Insert(new WorkPeriod() {
                WorkingDayId = lastestMockDate.Id,
                StartTime = lastestMockDate.Date.AddHours(hour).AddMinutes(min).AddSeconds(sec),
                Id = 11
            });
            var items = await repo.GetIncomplete();

            Assert.IsTrue(items.Count() == 1);
            Assert.IsTrue(items.FirstOrDefault()!.WorkPeriods.Count(x => !x.EndTime.HasValue) == 1);
        }

        [TestCase(6, 33, 1)]
        public async Task Repo_WorkPeriod_GetIncomplete_Test(int hour, int min, int sec) {

            var repo = _serviceProvider.GetService<IWorkPeriodRepository>();
            var lastestMockDate = MockWorkingDayRepository.Data.LastOrDefault();
            await _serviceProvider.GetService<IWriterWorkPeriod>().Insert(new WorkPeriod() {
                WorkingDayId = lastestMockDate.Id,
                StartTime = lastestMockDate.Date.AddHours(hour).AddMinutes(min).AddSeconds(sec),
                Id = 11
            });
            var items = await repo.GetIncomplete();

            Assert.IsTrue(items.Count() == 1);
            Assert.IsTrue(!items.FirstOrDefault().EndTime.HasValue);
        }
    }
}