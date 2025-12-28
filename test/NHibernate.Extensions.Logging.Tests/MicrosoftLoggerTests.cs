using System;
using Microsoft.Extensions.Logging;
using Xunit;

namespace NHibernate.Extensions.Logging.Tests;

public class MicrosoftLoggerTests
{
    private readonly MockLogger _mock;
    private readonly INHibernateLogger _logger;

    public MicrosoftLoggerTests()
    {
        _mock = new MockLogger();
        _logger = new MicrosoftLogger(_mock);
    }

    [Fact]
    public void LoggingWithMessage()
    {
        // Arrange
        var message = Guid.NewGuid().ToString();

        // Act
        _logger.Debug(message);

        // Assert
        Assert.IsType<NHibernateLogValues>(_mock.State);
        Assert.Equal(message, _mock.State.ToString());
        Assert.Equal(LogLevel.Debug, _mock.LogLevel);
        Assert.Null(_mock.Exception);
    }

    [Fact]
    public void LoggingWithException()
    {
        // Arrange
        var message = Guid.NewGuid().ToString();
        var exception = new Exception("Test");

        // Act
        _logger.Error(exception, message);

        // Assert
        Assert.IsType<NHibernateLogValues>(_mock.State);
        Assert.Equal(message, _mock.State.ToString());
        Assert.Equal(LogLevel.Error, _mock.LogLevel);
        Assert.Equal(exception, _mock.Exception);
    }

    [Fact]
    public void LoggingWithFormatter()
    {
        // Arrange
        var format = "Test {0}";
        var arg = Guid.NewGuid();

        // Act
        _logger.Info(format, arg);

        // Assert
        Assert.IsType<NHibernateLogValues>(_mock.State);
        Assert.Equal(string.Format(format, arg), _mock.State.ToString());
        Assert.Equal(LogLevel.Information, _mock.LogLevel);
        Assert.Null(_mock.Exception);
    }

    [Theory]
    [InlineData(NHibernateLogLevel.Trace)]
    [InlineData(NHibernateLogLevel.Debug)]
    [InlineData(NHibernateLogLevel.Info)]
    [InlineData(NHibernateLogLevel.Warn)]
    [InlineData(NHibernateLogLevel.Error)]
    [InlineData(NHibernateLogLevel.Fatal)]
    [InlineData(NHibernateLogLevel.None)]
    public void IsEnabledMapping(NHibernateLogLevel nhLevel)
    {
        // Act
        var result = _logger.IsEnabled(nhLevel);

        // Assert
        Assert.True(result); // MockLogger always returns true
    }

    [Theory]
    [InlineData(NHibernateLogLevel.Trace, LogLevel.Trace)]
    [InlineData(NHibernateLogLevel.Debug, LogLevel.Debug)]
    [InlineData(NHibernateLogLevel.Info, LogLevel.Information)]
    [InlineData(NHibernateLogLevel.Warn, LogLevel.Warning)]
    [InlineData(NHibernateLogLevel.Error, LogLevel.Error)]
    [InlineData(NHibernateLogLevel.Fatal, LogLevel.Critical)]
    [InlineData(NHibernateLogLevel.None, LogLevel.None)]
    public void LogLevelMapping(NHibernateLogLevel nhLevel, LogLevel msLevel)
    {
        // Arrange
        var message = "test";

        // Act
        _logger.Log(nhLevel, new NHibernateLogValues(message, null), null);

        // Assert
        Assert.Equal(msLevel, _mock.LogLevel);
    }
}