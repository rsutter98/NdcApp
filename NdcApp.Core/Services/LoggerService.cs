using System;
using Microsoft.Extensions.Logging;

namespace NdcApp.Core.Services
{
    /// <summary>
    /// Implementation of logger service that wraps Microsoft.Extensions.Logging.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        /// <summary>
        /// Initializes a new instance of the LoggerService class.
        /// </summary>
        /// <param name="logger">The Microsoft Extensions logger instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when logger is null.</exception>
        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public void LogDebug(string message)
        {
            _logger.LogDebug(message);
        }

        /// <inheritdoc />
        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        /// <inheritdoc />
        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        /// <inheritdoc />
        public void LogError(string message, Exception? exception = null)
        {
            if (exception != null)
            {
                _logger.LogError(exception, message);
            }
            else
            {
                _logger.LogError(message);
            }
        }

        /// <inheritdoc />
        public void LogError(Exception exception, string message)
        {
            _logger.LogError(exception, message);
        }
    }
}