using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Xunit;

namespace NHibernate.Extensions.Logging.Test
{
    public class MicrosoftLoggerTest
    {
        private readonly MockLogger _mock;
        private readonly IInternalLogger _logger;

        public MicrosoftLoggerTest()
        {
            _mock = new MockLogger();
            _logger = new MicrosoftLogger(_mock);
        }

        [Fact]
        public void LoggingWithMessage()
        {
            // Arrange
            object message = Guid.NewGuid().ToString();

            // Act
            _logger.Debug(message);

            // Assert
            Assert.IsType<FormattedLogValues>(_mock.State);
            Assert.Equal(message, _mock.State.ToString());
            Assert.Equal(LogLevel.Debug, _mock.LogLevel);
            Assert.Null(_mock.Exception);
        }

        [Fact]
        public void LoggingWithException()
        {
            // Arrange
            object message = Guid.NewGuid().ToString();
            var exception = new Exception("Test");

            // Act
            _logger.Error(message, exception);

            // Assert
            Assert.IsType<FormattedLogValues>(_mock.State);
            Assert.Equal(message, _mock.State.ToString());
            Assert.Equal(LogLevel.Error, _mock.LogLevel);
            Assert.Equal(exception, _mock.Exception);
        }

        [Fact]
        public void LoggingWithFormatter()
        {
            // Arrange
            var message = "Test {0}";
            var parameter = Guid.NewGuid();

            // Act
            _logger.InfoFormat(message, parameter);

            // Assert
            Assert.IsType<FormattedLogValues>(_mock.State);
            Assert.Equal(string.Format(message, parameter), _mock.State.ToString());
            Assert.Equal(LogLevel.Information, _mock.LogLevel);
            Assert.Null(_mock.Exception);
        }

        [Fact]
        public void LoggingWithObject()
        {
            // Arrange
            var exception = new Exception(Guid.NewGuid().ToString());

            // Act
            _logger.Warn(exception);

            // Assert
            Assert.IsType<FormattedLogValues>(_mock.State);
            Assert.Equal(exception.ToString(), _mock.State.ToString());
            Assert.Equal(LogLevel.Warning, _mock.LogLevel);
            Assert.Null(_mock.Exception);
        }
    }
}
