using System;

namespace Nybus.Logging
{
    public static class LogLevelExtensions
    {
        public static Microsoft.Extensions.Logging.LogLevel ToMicrosoftLogLevel(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Verbose:
                    return Microsoft.Extensions.Logging.LogLevel.Trace;

                case LogLevel.Debug:
                    return Microsoft.Extensions.Logging.LogLevel.Debug;

                case LogLevel.Information:
                    return Microsoft.Extensions.Logging.LogLevel.Information;

                case LogLevel.Warning:
                    return Microsoft.Extensions.Logging.LogLevel.Warning;

                case LogLevel.Error:
                    return Microsoft.Extensions.Logging.LogLevel.Error;

                case LogLevel.Critical:
                    return Microsoft.Extensions.Logging.LogLevel.Critical;
            }

            throw new NotSupportedException();
        }

        public static LogLevel ToNybusLogLevel(this Microsoft.Extensions.Logging.LogLevel level)
        {
            switch (level)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    return LogLevel.Verbose;

                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    return LogLevel.Debug;

                case Microsoft.Extensions.Logging.LogLevel.Information:
                    return LogLevel.Information;

                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    return LogLevel.Warning;

                case Microsoft.Extensions.Logging.LogLevel.Error:
                    return LogLevel.Error;

                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    return LogLevel.Critical;
            }

            throw new NotSupportedException();
        }
    }
}