namespace NdcApp.Core.Services
{
    /// <summary>
    /// Service interface for logging operations throughout the application.
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The debug message to log.</param>
        void LogDebug(string message);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The information message to log.</param>
        void LogInfo(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs an error message with an optional exception.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="exception">Optional exception associated with the error.</param>
        void LogError(string message, Exception? exception = null);

        /// <summary>
        /// Logs an error message with an associated exception.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="message">The error message to log.</param>
        void LogError(Exception exception, string message);
    }
}