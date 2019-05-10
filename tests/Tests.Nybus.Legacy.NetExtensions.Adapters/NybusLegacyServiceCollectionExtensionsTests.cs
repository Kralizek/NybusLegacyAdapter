using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;
using Nybus;
using Nybus.Configuration;
using Nybus.Container;
using Nybus.Utils;

namespace Tests
{
    [TestFixture]
    public class NybusLegacyServiceCollectionExtensionsTests
    {
        [Test, AutoMoqData]
        public void RegisterCommandHandler_registers_handler_as_transient(IServiceCollection services)
        {
            services.RegisterCommandHandler<TestCommand, TestCommandHandler>();

            Mock.Get(services).Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => sd.For<ICommandHandler<TestCommand>, TestCommandHandler>(ServiceLifetime.Transient))));
        }

        [Test, AutoMoqData]
        public void RegisterEventHandler_registers_handler_as_transient(IServiceCollection services)
        {
            services.RegisterEventHandler<TestEvent, TestEventHandler>();

            Mock.Get(services).Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => sd.For<IEventHandler<TestEvent>, TestEventHandler>(ServiceLifetime.Transient))));
        }

        [Test, AutoMoqData]
        public void AddNybusLegacy_registers_NybusLegacyHostedService(IServiceCollection services)
        {
            services.AddNybusLegacy();

            Mock.Get(services).Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => sd.For<IHostedService, NybusLegacyHostedService>())));
        }

        [Test, AutoMoqData]
        public void AddNybusLegacy_registers_ServiceProviderContainer(IServiceCollection services)
        {
            services.AddNybusLegacy();

            Mock.Get(services).Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => sd.For<IContainer, ServiceProviderContainer>())));
        }

        [Test, AutoMoqData]
        public void AddNybusLegacy_registers_Clock(IServiceCollection services)
        {
            services.AddNybusLegacy();

            Mock.Get(services).Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => sd.For<IClock>())));
        }

        [Test, AutoMoqData]
        public void AddNybusLegacy_registers_LoggerFactory(IServiceCollection services)
        {
            services.AddNybusLegacy();

            Mock.Get(services).Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => sd.For<Nybus.Logging.ILoggerFactory>())));
        }

        [Test, AutoMoqData]
        public void AddNybusLegacy_registers_NybusOptions(IServiceCollection services)
        {
            services.AddNybusLegacy();

            Mock.Get(services).Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => sd.For<NybusOptions>())));
        }

        [Test, AutoMoqData]
        public void AddNybusLegacy_registers_default_bus_engine(IServiceCollection services)
        {
            services.AddNybusLegacy();

            Mock.Get(services).Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => sd.For<IBusEngine>())));
        }

        [Test, AutoMoqData]
        public void AddNybusLegacy_registers_bus_builder(IServiceCollection services)
        {
            services.AddNybusLegacy();

            Mock.Get(services).Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => sd.For<IBusBuilder>())));
        }

        [Test, AutoMoqData]
        public void AddNybusLegacy_registers_bus(IServiceCollection services)
        {
            services.AddNybusLegacy();

            Mock.Get(services).Verify(p => p.Add(It.Is<ServiceDescriptor>(sd => sd.For<IBus>())));
        }
    }
}