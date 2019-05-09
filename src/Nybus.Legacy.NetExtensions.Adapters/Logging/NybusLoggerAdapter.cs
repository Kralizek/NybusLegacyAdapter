using System;
using System.Collections.Generic;
using INybusLogger = Nybus.Logging.ILogger;
using NybusLogLevel = Nybus.Logging.LogLevel;

namespace Nybus.Logging
{
    public class NybusLoggerAdapter : INybusLogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;

        public NybusLoggerAdapter(Microsoft.Extensions.Logging.ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Log(NybusLogLevel level, IDictionary<string, object> state, Exception exception)
        {
            if (state.TryGetValue("message", out var msg) && msg is string message)
            {
                _logger.Log(level.ToMicrosoftLogLevel(), 0, state, exception, (s, e) => message);
            }
        }

        public bool IsEnabled(NybusLogLevel level)
        {
            return _logger.IsEnabled(level.ToMicrosoftLogLevel());
        }
    }
}
