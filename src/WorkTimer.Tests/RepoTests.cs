using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Blazor.Extensions;
using WorkTimer.Contracts;

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
    }
}