using Xunit;
using Microsoft.Extensions.Logging;
using NdcApp.Core.Services;
using System;

namespace NdcApp.Tests
{
    public class LoggerServiceTests
    {
        [Fact]
        public void LoggerService_Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new LoggerService(null!));
        }

        [Fact]
        public void LoggerService_AllMethods_ShouldNotThrow()
        {
            // Arrange
            var factory = LoggerFactory.Create(builder => { });
            var logger = factory.CreateLogger<LoggerService>();
            var loggerService = new LoggerService(logger);

            // Act & Assert - Should not throw
            loggerService.LogDebug("Debug message");
            loggerService.LogInfo("Info message");
            loggerService.LogWarning("Warning message");
            loggerService.LogError("Error message");
            loggerService.LogError("Error message", new Exception("Test exception"));
            loggerService.LogError(new Exception("Test exception"), "Error message");
        }
    }
}