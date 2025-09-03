using Microsoft.Extensions.Logging;
using NdcApp.Core.Services;

namespace NdcApp.Services
{
    public class GlobalExceptionHandler
    {
        private readonly IErrorHandlingService _errorHandlingService;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(IErrorHandlingService errorHandlingService, ILogger<GlobalExceptionHandler> logger)
        {
            _errorHandlingService = errorHandlingService ?? throw new ArgumentNullException(nameof(errorHandlingService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> HandleUnhandledExceptionAsync(Exception exception, string context = "")
        {
            try
            {
                _logger.LogCritical(exception, "Unhandled exception occurred in context: {Context}", context);
                
                var handled = await _errorHandlingService.HandleErrorAsync(exception, $"Unhandled exception in {context}");
                
                // In a MAUI app, you might want to show a crash dialog or restart the app
                // For now, we'll just log and return whether it was handled
                return handled;
            }
            catch (Exception handlerException)
            {
                // If our error handler fails, log it but don't throw
                _logger.LogCritical(handlerException, "Error handler itself failed while handling: {OriginalException}", exception.Message);
                return false;
            }
        }

        public void RegisterGlobalHandlers()
        {
            // Register for unhandled app domain exceptions
            AppDomain.CurrentDomain.UnhandledException += async (sender, e) =>
            {
                if (e.ExceptionObject is Exception exception)
                {
                    await HandleUnhandledExceptionAsync(exception, "AppDomain.UnhandledException");
                }
            };

            // Register for unobserved task exceptions
            TaskScheduler.UnobservedTaskException += async (sender, e) =>
            {
                await HandleUnhandledExceptionAsync(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved(); // Prevent the app from crashing
            };
        }
    }
}