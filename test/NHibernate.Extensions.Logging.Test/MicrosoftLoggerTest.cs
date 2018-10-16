using System;
using Microsoft.Extensions.Logging;
using Xunit;

namespace NHibernate.Extensions.Logging.Test
{
    public class MicrosoftLoggerTest
    {
        private readonly MockLogger _mock;
        private readonly INHibernateLogger _logger;

        public MicrosoftLoggerTest()
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
    }
}
