using AutoFixture.NUnit3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Nybus;

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

            services.AddNybusWithMassTransit(nybusConfiguration);

            var serviceProvider = services.BuildServiceProvider();

            var bus = serviceProvider.GetRequiredService<IBus>();

            Assert.That(bus, Is.Not.Null);
        }
    }
}