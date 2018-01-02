using System;
using Microsoft.Extensions.Logging;

namespace NHibernate.Extensions.Logging
{
    /// <inheritdoc />
    /// <summary>
    /// Implements the <see cref="IInternalLogger"/> interface to allow the usage
    /// of Microsoft.Extensions.Logging with the NHibernate logging infrastructure.
    /// </summary>
    /// <seealso cref="MicrosoftLoggerFactory"/>
    public class MicrosoftLogger : IInternalLogger
    {
        private readonly ILogger _logger;

        public MicrosoftLogger(ILogger logger)
        {
            _logger = logger;
        }

        #region IInternalLogger

        public void Error(object message)
        {
            _logger.LogError(message.ToString());
        }

        public void Error(object message, Exception exception)
        {
            _logger.LogError(0, exception, message.ToString());
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _logger.LogError(format, args);
        }

        public void Fatal(object message)
        {
            _logger.LogCritical(message.ToString());
        }

        public void Fatal(object message, Exception exception)
        {
            _logger.LogCritical(0, exception, message.ToString());
        }

        public void Debug(object message)
        {
            _logger.LogDebug(message.ToString());
        }

        public void Debug(object message, Exception exception)
        {
            _logger.LogDebug(0, exception, message.ToString());
        }

        public void DebugFormat(string format, params object[] args)
        {
            _logger.LogDebug(format, args);
        }

        public void Info(object message)
        {
            _logger.LogInformation(message.ToString());
        }

        public void Info(object message, Exception exception)
        {
            _logger.LogInformation(0, exception, message.ToString());
        }

        public void InfoFormat(string format, params object[] args)
        {
            _logger.LogInformation(format, args);
        }

        public void Warn(object message)
        {
            _logger.LogWarning(message.ToString());
        }

        public void Warn(object message, Exception exception)
        {
            _logger.LogWarning(0, exception, message.ToString());
        }

        public void WarnFormat(string format, params object[] args)
        {
            _logger.LogWarning(format, args);
        }

        public bool IsErrorEnabled => _logger.IsEnabled(LogLevel.Error);
        public bool IsFatalEnabled => _logger.IsEnabled(LogLevel.Critical);
        public bool IsDebugEnabled => _logger.IsEnabled(LogLevel.Debug);
        public bool IsInfoEnabled => _logger.IsEnabled(LogLevel.Information);
        public bool IsWarnEnabled => _logger.IsEnabled(LogLevel.Warning);

        #endregion
    }
}
