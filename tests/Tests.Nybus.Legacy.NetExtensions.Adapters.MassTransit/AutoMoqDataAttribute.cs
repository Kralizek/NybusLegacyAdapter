using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nybus.Logging;
using Nybus.MassTransit;

namespace Tests
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base (CreateFixture)
        {
            
        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoMoqCustomization
            {
                ConfigureMembers = true,
                GenerateDelegates = true
            });

            fixture.Customize<NybusLoggerFactoryAdapter>(o => o.Without(p => p.MinimumLevel));

            fixture.Customize<IConfiguration>(o => o.FromFactory((ConfigurationBuilder configurationBuilder, MassTransitConnectionDescriptor connectionDescriptor, string connectionStringName) =>
            {
                configurationBuilder.AddInMemoryCollection(new Dictionary<string, string>
                {
                    [$"ConnectionStrings:{connectionStringName}"] = connectionDescriptor.ToConnectionString()
                });

                var configuration = configurationBuilder.Build() as IConfiguration;

                return configuration;
            }));

            return fixture;
        }
    }

    public static class TestHelpers
    {
        public static bool For<TService, TImplementation>(this ServiceDescriptor descriptor, ServiceLifetime lifetime)
            where TImplementation : class, TService
        {
            return descriptor.ServiceType == typeof(TService) && descriptor.ImplementationType == typeof(TImplementation) && descriptor.Lifetime == lifetime;
        }

        public static bool For<TService, TImplementation>(this ServiceDescriptor descriptor)
            where TImplementation : class, TService
        {
            return descriptor.ServiceType == typeof(TService) && descriptor.ImplementationType == typeof(TImplementation);
        }

        public static bool For<TService>(this ServiceDescriptor descriptor)
        {
            return descriptor.ServiceType == typeof(TService);
        }

        public static string ToConnectionString(this MassTransitConnectionDescriptor connectionDescriptor)
        {
            return $"host={connectionDescriptor.Host};username={connectionDescriptor.UserName};password={connectionDescriptor.Password}";
        }
    }
}