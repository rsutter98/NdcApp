using Xunit;
using NdcApp.Core.Services;
using NdcApp.Tests.Mocks;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace NdcApp.Tests
{
    public class ErrorHandlingServiceTests
    {
        private readonly ErrorHandlingService _errorService;
        private readonly MockLoggerService _mockLogger;

        public ErrorHandlingServiceTests()
        {
            _mockLogger = new MockLoggerService();
            _errorService = new ErrorHandlingService(_mockLogger);
        }

        [Fact]
        public async Task HandleErrorAsync_ShouldLogError_ReturnsTrue()
        {
            // Arrange
            var exception = new ArgumentException("Test error");
            var context = "Test context";

            // Act
            var result = await _errorService.HandleErrorAsync(exception, context);

            // Assert
            Assert.True(result);
            Assert.Contains(_mockLogger.LogEntries, entry => entry.Contains("ERROR"));
            Assert.Contains(_mockLogger.LogEntries, entry => entry.Contains("Test error"));
        }

        [Fact]
        public void GetUserFriendlyMessage_FileNotFoundException_ReturnsAppropriateMessage()
        {
            // Arrange
            var exception = new FileNotFoundException("File not found");

            // Act
            var message = _errorService.GetUserFriendlyMessage(exception);

            // Assert
            Assert.Contains("required file could not be found", message);
        }

        [Fact]
        public void GetUserFriendlyMessage_HttpRequestException_ReturnsNetworkMessage()
        {
            // Arrange
            var exception = new HttpRequestException("Network error");

            // Act
            var message = _errorService.GetUserFriendlyMessage(exception);

            // Assert
            Assert.Contains("Network error", message);
            Assert.Contains("internet connection", message);
        }

        [Fact]
        public void GetUserFriendlyMessage_ArgumentNullException_ReturnsInvalidInputMessage()
        {
            // Arrange
            var exception = new ArgumentNullException("parameter");

            // Act
            var message = _errorService.GetUserFriendlyMessage(exception);

            // Assert
            Assert.Contains("Invalid input", message);
        }

        [Fact]
        public void GetUserFriendlyMessage_UnknownException_ReturnsGenericMessage()
        {
            // Arrange
            var exception = new InvalidCastException("Cast error");

            // Act
            var message = _errorService.GetUserFriendlyMessage(exception);

            // Assert
            Assert.Contains("unexpected error occurred", message);
        }

        [Fact]
        public void LogError_WithContext_LogsContextualMessage()
        {
            // Arrange
            var exception = new Exception("Test error");
            var context = "Test method";

            // Act
            _errorService.LogError(exception, context);

            // Assert
            Assert.Contains(_mockLogger.LogEntries, entry => 
                entry.Contains("ERROR") && 
                entry.Contains("Test method") && 
                entry.Contains("Test error"));
        }

        [Fact]
        public void LogError_WithoutContext_LogsBasicMessage()
        {
            // Arrange
            var exception = new Exception("Test error");

            // Act
            _errorService.LogError(exception);

            // Assert
            Assert.Contains(_mockLogger.LogEntries, entry => 
                entry.Contains("ERROR") && 
                entry.Contains("Error occurred") && 
                entry.Contains("Test error"));
        }
    }
}