using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nybus.Logging;

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
    }
}