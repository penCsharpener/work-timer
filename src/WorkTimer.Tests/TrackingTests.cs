using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using WorkTimer.Blazor.Extensions;

namespace WorkTimer.Tests {

    [TestFixture]
    public class TrackingTests {

        private IServiceProvider _serviceProvider;

        public TrackingTests() {
            var services = new ServiceCollection();
            services.WireUpMockClasses();

            _serviceProvider = services.BuildServiceProvider();
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
