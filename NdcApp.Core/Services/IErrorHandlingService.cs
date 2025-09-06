using System;
using System.Threading.Tasks;

namespace NdcApp.Core.Services
{
    /// <summary>
    /// Service interface for handling and managing application errors and exceptions.
    /// </summary>
    public interface IErrorHandlingService
    {
        /// <summary>
        /// Handles an exception by logging it and optionally showing a user-friendly message.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="userMessage">Optional user-friendly message to display. If empty, a default message is generated.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates if the error was handled successfully.</returns>
        Task<bool> HandleErrorAsync(Exception exception, string userMessage = "");

        /// <summary>
        /// Converts an exception into a user-friendly error message.
        /// </summary>
        /// <param name="exception">The exception to convert.</param>
        /// <returns>A user-friendly error message.</returns>
        string GetUserFriendlyMessage(Exception exception);

        /// <summary>
        /// Logs an error with optional context information.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">Optional context information about where the error occurred.</param>
        void LogError(Exception exception, string context = "");
    }
}