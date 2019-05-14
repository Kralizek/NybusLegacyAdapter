using AutoFixture.NUnit3;
using Microsoft.Extensions.Configuration;
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
        public void Bus_can_be_built([Frozen] string connectionStringName, IConfiguration configuration, ServiceCollection services, NybusLegacyConfiguration nybusConfiguration)
        {
            services.AddSingleton(configuration);

            services.AddLogging();

            services.AddNybusLegacyWithMassTransit(nybusConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var bus = serviceProvider.GetRequiredService<IBus>();

            Assert.That(bus, Is.Not.Null);
        }

        [Test, AutoMoqData]
        public void SubscriptionsConfigurator_is_used_when_building_bus([Frozen] string connectionStringName, IConfiguration configuration, ServiceCollection services, NybusLegacyConfiguration nybusConfiguration)
        {
            services.AddSingleton(configuration);

            services.AddLogging();

            services.AddNybusLegacyWithMassTransit(nybusConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var bus = serviceProvider.GetRequiredService<IBus>();

            Mock.Get(nybusConfiguration.SubscriptionsConfigurator).Verify(p => p(It.IsAny<IBusBuilder>()));
        }
        
        [Test, AutoMoqData]
        public void NybusConfigurator_is_used_when_building_bus([Frozen] string connectionStringName, IConfiguration configuration, ServiceCollection services, NybusLegacyConfiguration nybusConfiguration)
        {
            services.AddSingleton(configuration);

            services.AddLogging();

            services.AddNybusLegacyWithMassTransit(nybusConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var bus = serviceProvider.GetRequiredService<IBus>();

            Mock.Get(nybusConfiguration.NybusConfigurator).Verify(p => p(It.IsAny<NybusOptions>()));
        }

        [Test, AutoMoqData]
        public void MassTransitConfigurator_is_used_when_building_bus([Frozen] string connectionStringName, IConfiguration configuration, ServiceCollection services, NybusLegacyConfiguration nybusConfiguration)
        {
            services.AddSingleton(configuration);

            services.AddLogging();

            services.AddNybusLegacyWithMassTransit(nybusConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var bus = serviceProvider.GetRequiredService<IBus>();

            Mock.Get(nybusConfiguration.NybusConfigurator).Verify(p => p(It.IsAny<NybusOptions>()));
        }
    }
}