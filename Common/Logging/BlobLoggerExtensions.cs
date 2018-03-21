using System;
using Common.Logging.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.Logging
{
    public static class BlobLoggerExtensions
    {
        /// <summary>
        /// Adds a Blob logger that is enabled for log level Information and higher
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="loggerConfiguration"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static ILoggerFactory AddBlob(this ILoggerFactory factory,
            BlobLoggerConfiguration loggerConfiguration, Func<string, LogLevel, bool> filter = null)
        {
            if (filter == null)
                filter = (_, level) => level >= LogLevel.Information;
            
            factory.AddProvider(new BlobLoggingProvider(filter, loggerConfiguration));
            return factory;
        }

        public static ILoggerFactory AddBlob(this ILoggerFactory factory, BlobLoggerConfiguration loggerConfiguration, LogLevel minLevel)
        {
            return AddBlob(
                factory,
                loggerConfiguration,
                (_, logLevel) => logLevel >= minLevel);
        }
    }
}
