using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Nybus;
using Nybus.Configuration;

namespace Tests
{
    [TestFixture]
    public class NybusLegacyServiceCollectionExtensionsIntegrationTests
    {
        [Test, AutoMoqData]
        public void Bus_can_be_built(ServiceCollection services, NybusConfigurator nybusConfigurator, SubscriptionsConfigurator subscriptionsConfigurator)
        {
            services.AddLogging();

            services.AddNybusLegacy(nybusConfigurator, subscriptionsConfigurator);

            var serviceProvider = services.BuildServiceProvider();

            var bus = serviceProvider.GetRequiredService<IBus>();

            Assert.That(bus, Is.Not.Null);
        }

        [Test, AutoMoqData]
        public void NybusConfigurator_delegate_is_invoked(ServiceCollection services, NybusConfigurator nybusConfigurator, SubscriptionsConfigurator subscriptionsConfigurator)
        {
            services.AddLogging();

            services.AddNybusLegacy(nybusConfigurator, subscriptionsConfigurator);

            var serviceProvider = services.BuildServiceProvider();

            var bus = serviceProvider.GetRequiredService<IBus>();

            Assume.That(bus, Is.Not.Null);

            Mock.Get(nybusConfigurator).Verify(p => p(It.IsAny<INybusOptions>()));
        }

        [Test, AutoMoqData]
        public void SubscriptionsConfigurator_delegate_is_invoked(ServiceCollection services, NybusConfigurator nybusConfigurator, SubscriptionsConfigurator subscriptionsConfigurator)
        {
            services.AddLogging();

            services.AddNybusLegacy(nybusConfigurator, subscriptionsConfigurator);

            var serviceProvider = services.BuildServiceProvider();

            var bus = serviceProvider.GetRequiredService<IBus>();

            Assume.That(bus, Is.Not.Null);

            Mock.Get(subscriptionsConfigurator).Verify(p => p(It.IsAny<IBusBuilder>()));
        }
    }
}