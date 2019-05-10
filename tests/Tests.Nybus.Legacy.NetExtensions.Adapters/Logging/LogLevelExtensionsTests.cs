using System;
using NUnit.Framework;
using Nybus.Logging;
using NybusLogLevel = Nybus.Logging.LogLevel;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Tests.Logging
{
    [TestFixture]
    public class LogLevelExtensionsTests
    {
        [Test]
        [TestCase(NybusLogLevel.Verbose, LogLevel.Trace)]
        [TestCase(NybusLogLevel.Debug, LogLevel.Debug)]
        [TestCase(NybusLogLevel.Information, LogLevel.Information)]
        [TestCase(NybusLogLevel.Warning, LogLevel.Warning)]
        [TestCase(NybusLogLevel.Error, LogLevel.Error)]
        [TestCase(NybusLogLevel.Critical, LogLevel.Critical)]

        public void ToMicrosoftLogLevel_converts_to_right_value(NybusLogLevel nybus, LogLevel logLevel)
        {
            Assert.That(nybus.ToMicrosoftLogLevel(), Is.EqualTo(logLevel));
        }

        [Test, AutoMoqData]
        public void ToMicrosoftLogLevel_throws_if_invalid_value(int invalidValue)
        {
            var logLevel = (NybusLogLevel)invalidValue;

            Assume.That(logLevel, Is.GreaterThan(NybusLogLevel.Critical));

            Assert.Throws<NotSupportedException>(() => logLevel.ToMicrosoftLogLevel());
        }

        [Test]
        [TestCase(LogLevel.Trace, NybusLogLevel.Verbose)]
        [TestCase(LogLevel.Debug, NybusLogLevel.Debug)]
        [TestCase(LogLevel.Information, NybusLogLevel.Information)]
        [TestCase(LogLevel.Warning, NybusLogLevel.Warning)]
        [TestCase(LogLevel.Error, NybusLogLevel.Error)]
        [TestCase(LogLevel.Critical, NybusLogLevel.Critical)]
        public void ToNybusLogLevel_converts_to_right_value(LogLevel logLevel, NybusLogLevel nybus)
        {
            Assert.That(logLevel.ToNybusLogLevel(), Is.EqualTo(nybus));
        }

        [Test]
        public void None_is_not_supported_by_ToNybusLogLevel()
        {
            Assert.Throws<NotSupportedException>(() => LogLevel.None.ToNybusLogLevel());
        }

        [Test, AutoMoqData]
        public void ToNybusLogLevel_throws_if_invalid_value(int invalidValue)
        {
            var logLevel = (LogLevel)invalidValue;

            Assume.That(logLevel, Is.GreaterThan(LogLevel.Critical));

            Assert.Throws<NotSupportedException>(() => logLevel.ToNybusLogLevel());
        }
    }
}
