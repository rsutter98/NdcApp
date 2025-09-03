namespace NdcApp.Core.Services
{
    public interface ILoggerService
    {
        void LogDebug(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception? exception = null);
        void LogError(Exception exception, string message);
    }
}