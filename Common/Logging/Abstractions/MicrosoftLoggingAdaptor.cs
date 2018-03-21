using System;
using Microsoft.Extensions.Logging;

namespace Common.Logging.Abstractions
{
    //https://stackoverflow.com/questions/41243485/simple-injector-register-iloggert-by-using-iloggerfactory-createloggert
    public interface ILogger
    {
        void Log(LogEntry entry);
        void Log(Exception exception);
        void Log(string message, LoggingEventType loggingEventType = LoggingEventType.Information);
    }

    public enum LoggingEventType { Debug, Information, Warning, Error, Fatal };

    // Immutable DTO that contains the log information.
    public class LogEntry
    {
        public readonly LoggingEventType Severity;
        public readonly string Message;
        public readonly Exception Exception;

        public LogEntry(LoggingEventType severity, string message, Exception exception = null)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (message == string.Empty) throw new ArgumentException("empty", "message");

            Severity = severity;
            Message = message;
            Exception = exception;
        }
    }

    public sealed class MicrosoftLoggingAdapter<T> : MicrosoftLoggingAdapter
    {
        public MicrosoftLoggingAdapter(Microsoft.Extensions.Logging.ILoggerFactory factory)
            : base(factory.CreateLogger<T>()) { }
    }

    public class MicrosoftLoggingAdapter : ILogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger adaptee;

        public MicrosoftLoggingAdapter(Microsoft.Extensions.Logging.ILogger adaptee) =>
            this.adaptee = adaptee;

        public void Log(LogEntry e) =>
            adaptee.Log(ToLevel(e.Severity), 0, e.Message, e.Exception, (s, _) => s);

        public void Log(Exception exception)
        {            
            Log(new LogEntry(LoggingEventType.Error, exception.Message, exception));            
        }

        public void Log(string message, LoggingEventType loggingEventType = LoggingEventType.Information)
        {
            Log(new LogEntry(loggingEventType, message));
        }

        private static LogLevel ToLevel(LoggingEventType s) =>
            s == LoggingEventType.Debug ? LogLevel.Debug :
            s == LoggingEventType.Information ? LogLevel.Information :
            s == LoggingEventType.Warning ? LogLevel.Warning :
            s == LoggingEventType.Error ? LogLevel.Error :
            LogLevel.Critical;
    }
}
