using System;
using System.Collections.Generic;
using INybusLoggerFactory = Nybus.Logging.ILoggerFactory;
using INybusLoggerProvider = Nybus.Logging.ILoggerProvider;
using INybusLogger = Nybus.Logging.ILogger;
using NybusLogLevel = Nybus.Logging.LogLevel;


namespace Nybus.Logging
{
    public class NybusLoggerFactoryAdapter : INybusLoggerFactory
    {
        private readonly Microsoft.Extensions.Logging.ILoggerFactory _loggerFactory;

        public NybusLoggerFactoryAdapter(Microsoft.Extensions.Logging.ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public void Dispose()
        {
            _loggerFactory.Dispose();
        }

        public INybusLogger CreateLogger(string categoryName)
        {
            var logger = _loggerFactory.CreateLogger(categoryName);
            return new NybusLoggerAdapter(logger);
        }

        public void AddProvider(INybusLoggerProvider provider)
        {
            throw new NotSupportedException();
        }

        public IReadOnlyList<INybusLoggerProvider> GetProviders()
        {
            throw new NotSupportedException();
        }

        public NybusLogLevel MinimumLevel
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}