using System;
using System.Collections.Generic;
using AutoFixture.Idioms;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Nybus.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using LogLevel = Nybus.Logging.LogLevel;
using MicrosoftLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Tests.Logging
{
    [TestFixture]
    public class NybusLoggerAdapterTests
    {
        [Test, AutoMoqData]
        public void Constructor_is_guarded(GuardClauseAssertion assertion)
        {
            assertion.Verify(typeof(NybusLoggerAdapter).GetConstructors());
        }

        [Test, AutoMoqData]
        public void Log_forwards_to_Microsoft_Logger_if_message_is_specified([Frozen] ILogger logger, NybusLoggerAdapter sut, LogLevel level, Dictionary<string, object> data, Exception exception, string message)
        {
            data.Add("message", message);

            sut.Log(level, data, exception);

            Mock.Get(logger).Verify(p => p.Log(It.IsAny<MicrosoftLogLevel>(), It.IsAny<EventId>(), data, exception, It.IsAny<Func<IDictionary<string, object>, Exception, string>>()));
        }

        [Test, AutoMoqData]
        public void Log_doesnt_forward_to_Microsoft_Logger_if_message_is_not_specified([Frozen] ILogger logger, NybusLoggerAdapter sut, LogLevel level, Dictionary<string, object> data, Exception exception)
        {
            sut.Log(level, data, exception);

            Mock.Get(logger).Verify(p => p.Log(It.IsAny<MicrosoftLogLevel>(), It.IsAny<EventId>(), It.IsAny<IDictionary<string, object>>(), It.IsAny<Exception>(), It.IsAny<Func<IDictionary<string, object>, Exception, string>>()), Times.Never);
        }

        [Test, AutoMoqData]
        public void IsEnabled_forwards_to_Microsoft_Logger([Frozen] ILogger logger, NybusLoggerAdapter sut, LogLevel level)
        {
            sut.IsEnabled(level);

            Mock.Get(logger).Verify(p => p.IsEnabled(It.IsAny<MicrosoftLogLevel>()));
        }
    }
}