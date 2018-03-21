using System;
using Common.Logging.Configuration;
using Microsoft.Extensions.Logging;

namespace Common.Logging
{
    //https://wildermuth.com/2016/04/22/Implementing-an-ASP-NET-Core-RC1-Logging-Provider
    //http://intellitect.com/implementing-a-custom-ilogger-with-exception-handling-for-net-core/
    //https://msdn.microsoft.com/en-us/magazine/mt694089.aspx

    public class BlobLoggingProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> filter;
        private readonly BlobLoggerConfiguration loggerConfiguration;

        public BlobLoggingProvider(Func<string, LogLevel, bool> filter, BlobLoggerConfiguration loggerConfiguration)
        {
            this.filter = filter;            
            this.loggerConfiguration = loggerConfiguration;
        }

        public void Dispose()
        {
            
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
        {
            return new BlobLogger(categoryName, filter, loggerConfiguration);
        }
    }
}
