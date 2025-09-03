using System.IO;
using System.Net;

namespace NdcApp.Core.Services
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        private readonly ILoggerService _logger;

        public ErrorHandlingService(ILoggerService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> HandleErrorAsync(Exception exception, string userMessage = "")
        {
            LogError(exception, userMessage);

            var friendlyMessage = GetUserFriendlyMessage(exception);
            
            // In a MAUI app, we would typically show an alert or notification
            // This is just a placeholder for the error handling logic
            await Task.CompletedTask;
            
            return true; // Indicates error was handled
        }

        public string GetUserFriendlyMessage(Exception exception)
        {
            return exception switch
            {
                FileNotFoundException => "A required file could not be found. Please try refreshing the data.",
                UnauthorizedAccessException => "Access denied. Please check your permissions.",
                HttpRequestException => "Network error. Please check your internet connection and try again.",
                TimeoutException => "The operation timed out. Please try again.",
                ArgumentNullException => "Invalid input provided. Please check your data and try again.",
                ArgumentException => "Invalid input provided. Please check your data and try again.",
                DirectoryNotFoundException => "A required directory could not be found.",
                IOException => "A file operation failed. Please try again.",
                _ => "An unexpected error occurred. Please try again or contact support if the problem persists."
            };
        }

        public void LogError(Exception exception, string context = "")
        {
            var message = string.IsNullOrEmpty(context) 
                ? $"Error occurred: {exception.Message}" 
                : $"Error in {context}: {exception.Message}";
                
            _logger.LogError(exception, message);
        }
    }
}