using NdcApp.Core.Services;

namespace NdcApp.Tests.Mocks
{
    public class MockLoggerService : ILoggerService
    {
        public List<string> LogEntries { get; } = new List<string>();

        public void LogDebug(string message)
        {
            LogEntries.Add($"DEBUG: {message}");
        }

        public void LogInfo(string message)
        {
            LogEntries.Add($"INFO: {message}");
        }

        public void LogWarning(string message)
        {
            LogEntries.Add($"WARNING: {message}");
        }

        public void LogError(string message, Exception? exception = null)
        {
            var entry = $"ERROR: {message}";
            if (exception != null)
            {
                entry += $" | Exception: {exception.Message}";
            }
            LogEntries.Add(entry);
        }

        public void LogError(Exception exception, string message)
        {
            LogEntries.Add($"ERROR: {message} | Exception: {exception.Message}");
        }

        public void Clear()
        {
            LogEntries.Clear();
        }
    }
}