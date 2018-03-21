using System;
using System.IO;
using System.Text;
using Common.Logging.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Common.Logging
{
    public class BlobLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly string categoryName;
        private readonly Func<string, Microsoft.Extensions.Logging.LogLevel, bool> filter;
        private readonly BlobLoggerConfiguration loggerConfiguration;
        private readonly CloudBlobContainer cloudBlobContainer;

        public BlobLogger(string categoryName, Func<string, Microsoft.Extensions.Logging.LogLevel, bool> filter, BlobLoggerConfiguration loggerConfiguration)
        {
            this.categoryName = categoryName;
            this.filter = filter;
            this.loggerConfiguration = loggerConfiguration;

            CloudStorageAccount account =  CloudStorageAccount.Parse(loggerConfiguration.ConnectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            cloudBlobContainer = client.GetContainerReference(loggerConfiguration.Container.ToLower());
            cloudBlobContainer.CreateIfNotExistsAsync().Wait();
        }

        public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            message = $"[{DateTime.Now}] { logLevel }: {message}";

            if (exception != null)
            {
                message += Environment.NewLine + Environment.NewLine + exception;
            }

            StringBuilder sb = new StringBuilder();                 
            sb.Append(loggerConfiguration.FileName);

            CloudAppendBlob appendBlob = cloudBlobContainer.GetAppendBlobReference(sb.ToString());
            if (!appendBlob.ExistsAsync().Result)
                appendBlob.CreateOrReplaceAsync().Wait();

            appendBlob.AppendTextAsync(message + Environment.NewLine);            
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            return filter == null || filter(categoryName, logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}
