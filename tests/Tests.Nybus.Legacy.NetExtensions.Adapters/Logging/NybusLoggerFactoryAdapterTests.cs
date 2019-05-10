using System;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using Nybus.Logging;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

namespace Tests.Logging
{
    [TestFixture]
    public class NybusLoggerFactoryAdapterTests
    {
        [Test, AutoMoqData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(NybusLoggerFactoryAdapter).GetConstructors());
        }

        [Test, AutoMoqData]
        public void Dispose_is_forwarded_to_inner_factory([Frozen] ILoggerFactory loggerFactory, NybusLoggerFactoryAdapter sut)
        {
            sut.Dispose();

            Mock.Get(loggerFactory).Verify(p => p.Dispose());
        }

        [Test, AutoMoqData]
        public void CreateLogger_forwards_to_inner_factory_and_returns_adapter([Frozen] ILoggerFactory loggerFactory, NybusLoggerFactoryAdapter sut, string categoryName)
        {
            var logger = sut.CreateLogger(categoryName);

            Assert.That(logger,Is.InstanceOf<NybusLoggerAdapter>());

            Mock.Get(loggerFactory).Verify(p => p.CreateLogger(categoryName));
        }

        [Test, AutoMoqData]
        public void AddProvider_is_not_supported(NybusLoggerFactoryAdapter sut, ILoggerProvider provider)
        {
            Assert.Throws<NotSupportedException>(() => sut.AddProvider(provider));
        }

        [Test, AutoMoqData]
        public void GetProviders_is_not_supported(NybusLoggerFactoryAdapter sut)
        {
            Assert.Throws<NotSupportedException>(() => sut.GetProviders());
        }

        [Test, AutoMoqData]
        public void MinimumLevel_property_setter_is_not_supported(NybusLoggerFactoryAdapter sut, LogLevel level)
        {
            Assert.Throws<NotSupportedException>(() => sut.MinimumLevel = level);
        }

        [Test, AutoMoqData]
        public void MinimumLevel_property_getter_is_not_supported(NybusLoggerFactoryAdapter sut)
        {
            Assert.Throws<NotSupportedException>(() => sut.MinimumLevel.ToString());
        }
    }
}