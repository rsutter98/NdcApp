namespace NdcApp.Core.Services
{
    public interface IErrorHandlingService
    {
        Task<bool> HandleErrorAsync(Exception exception, string userMessage = "");
        string GetUserFriendlyMessage(Exception exception);
        void LogError(Exception exception, string context = "");
    }
}