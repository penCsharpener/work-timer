using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tynamix.ObjectFiller;
using WorkTimer.Config;
using WorkTimer.Contracts;
using WorkTimer.Models;
using WorkTimer.Repositories;
using WorkTimer.Services;

namespace WorkTimer.Tests {

    [TestFixture]
    public class DbTests {

        private IServiceProvider _serviceProvider;
        private IConfigurationRoot _config;

        public DbTests() {
            var services = new ServiceCollection();

            services.AddTransient<IWorkPeriodWriter, WorkPeriodWriter>();
            services.AddTransient<IWorkPeriodRepository, WorkPeriodRepository>();
            services.AddTransient<IToggleTracking, ToggleTrackingService>();
            services.AddTransient<IWriterWorkPeriod, WriterWorkPeriod>();
            services.AddTransient<IWorkingDayRepository, WorkingDayRepository>();
            services.AddSingleton<IDatabaseConnection<SQLiteConnection>, SqliteDatabaseConnectionService>();

            var conf = new ConfigurationBuilder();
            _config = conf.SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(WorkTimer.Blazor.Program))?.Location))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<WorkTimer.Blazor.Program>(optional: true, reloadOnChange: true)
            .Build();

            services.Configure<SqliteConfiguration>(_config.GetSection(nameof(SqliteConfiguration)));

            _serviceProvider = services.BuildServiceProvider();
        }


        [TestCase]
        public async Task Init_Database() {
            var conService = _serviceProvider.GetService<IDatabaseConnection<SQLiteConnection>>();
            var options = _serviceProvider.GetService<IOptions<SqliteConfiguration>>();
            var dbService = new DbInitService(conService, options);
            await dbService.InitializeDatabase();
            Assert.IsTrue(File.Exists(options.Value.DatabaseFullPath));
        }

        [Test]
        public void DatabaseExists() {
            var options = _serviceProvider.GetService<IOptions<SqliteConfiguration>>();

            Assert.IsTrue(File.Exists(options.Value.DatabaseFullPath));
        }

        [Test]
        public async Task Insert() {
            var writer = _serviceProvider.GetService<IWorkPeriodWriter>();

            var filler = new Filler<WorkPeriod>();
            filler.Setup()
                  .OnProperty(x => x.Id).Use(0)
                  .OnProperty(x => x.Comment).Use(new MnemonicString(5, 3, 12))
                  .OnProperty(x => x.StartTime).Use(DateTime.Now);

            var obj = filler.Create();
            var orgId = obj.Id;
            var item = await writer.Insert(obj);
            Assert.IsTrue(orgId < item.Id);
        }

        [Test]
        public async Task GetAll() {
            var repo = _serviceProvider.GetService<IWorkPeriodRepository>();

            var items = await repo.GetAll();
            Assert.IsTrue(items.Any());
        }

        [Test]
        public async Task Delete() {
            var repo = _serviceProvider.GetService<IWorkPeriodRepository>();
            var writer = _serviceProvider.GetService<IWorkPeriodWriter>();

            var items = await repo.GetAll();
            var countBefore = items.Count();
            await writer.Delete(items.FirstOrDefault());
            var countAfter = (await repo.GetAll()).Count();
            Assert.IsTrue(countBefore - countAfter == 1);
        }

        [Test]
        public async Task Update() {
            var repo = _serviceProvider.GetService<IWorkPeriodRepository>();
            var writer = _serviceProvider.GetService<IWorkPeriodWriter>();

            var items = await repo.GetAll();
            var item = items.FirstOrDefault();
            var itemId = item.Id;
            var startTimeFirst = item.StartTime.AddDays(1);
            await writer.Update(item.Id, startTimeFirst, item.EndTime, item.IsBreak, item.Comment, item.ExpectedHours);
            var itemUpdated = await repo.FindById(itemId);
            Assert.IsTrue(startTimeFirst == itemUpdated.StartTime);
        }

        [Test]
        public async Task Seeding() {
            var list = new List<WorkPeriod>() {
            };

            var writer = _serviceProvider.GetService<IWorkPeriodWriter>();

            foreach (var item in list) {
                await writer.Insert(item);
            }
            var repo = _serviceProvider.GetService<IWorkPeriodRepository>();
            var items = await repo.GetAll();
            Assert.IsTrue(items.Any());
        }

        [Test]
        public async Task GetTotalOverhours() {
            var dayRepo = _serviceProvider.GetService<IWorkingDayRepository>();
            var list = await dayRepo.GetAll();

            double seconds = 0;
            foreach (var item in list) {
                seconds += item.Overhours.TotalSeconds;
            }
            var quickTotal = await dayRepo.GetTotalOverhours();
            Assert.AreEqual(quickTotal.TotalSeconds, seconds);
        }
    }
    /*
        public class DbTests2 {

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
