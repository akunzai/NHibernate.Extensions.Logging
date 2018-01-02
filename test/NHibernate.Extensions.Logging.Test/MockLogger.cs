using System;
using Microsoft.Extensions.Logging;

namespace NHibernate.Extensions.Logging.Test
{
    public class MockLogger : ILogger
    {
        public object State { get; set; }
        public LogLevel LogLevel { get; set; }
        public Exception Exception { get; set; }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogLevel = logLevel;
            State = state;
            Exception = exception;
        }
    }
}
