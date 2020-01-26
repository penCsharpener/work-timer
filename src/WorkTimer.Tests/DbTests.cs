using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;
using WorkTimer.Config;
using WorkTimer.Repositories;

namespace WorkTimer.Tests {

    [TestFixture]
    public class DbTests {

        private IServiceProvider _serviceProvider;
        private IConfigurationRoot _config;

        public DbTests() {
            var services = new ServiceCollection();
            var conf = new ConfigurationBuilder();
            _config = conf.SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(WorkTimer.Blazor.Program))?.Location))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets<WorkTimer.Blazor.Program>(optional: true, reloadOnChange: true)
            .Build();

            services.Configure<SqliteConfiguration>(_config.GetSection(nameof(SqliteConfiguration)));

            _serviceProvider = services.BuildServiceProvider();
        }


        [TestCase()]
        public async Task TestTemplateAsync() {
            var options = _serviceProvider.GetService<IOptions<SqliteConfiguration>>();
            var dbService = new DbInitService(options);
            await dbService.InitializeDatabase();
            Assert.IsTrue(true);
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
