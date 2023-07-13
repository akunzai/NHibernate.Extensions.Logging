using System;
using Microsoft.Extensions.Logging;

namespace NHibernate.Extensions.Logging.Tests;

public class MockLogger : ILogger
{
    public MockLogger()
    {
    }

    public MockLogger(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

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