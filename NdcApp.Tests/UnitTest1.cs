using Xunit;
using NdcApp.Core.Models;
using System;

namespace NdcApp.Tests
{
    public class TalkModelTests
    {
        [Fact]
        public void Talk_DefaultConstructor_SetsEmptyValues()
        {
            // Arrange & Act
            var talk = new Talk();

            // Assert
            Assert.Equal(string.Empty, talk.Day);
            Assert.Equal(string.Empty, talk.Room);
            Assert.Equal(string.Empty, talk.Title);
            Assert.Equal(string.Empty, talk.Speaker);
            Assert.Equal(string.Empty, talk.Category);
            Assert.Equal(TimeSpan.Zero, talk.StartTime);
            Assert.Equal(TimeSpan.Zero, talk.EndTime);
        }

        [Fact]
        public void Talk_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var talk = new Talk();
            var expectedDay = "Wednesday";
            var expectedStartTime = new TimeSpan(9, 0, 0);
            var expectedEndTime = new TimeSpan(10, 0, 0);
            var expectedRoom = "Room 1";
            var expectedTitle = "Test Talk";
            var expectedSpeaker = "John Doe";
            var expectedCategory = "Technology";

            // Act
            talk.Day = expectedDay;
            talk.StartTime = expectedStartTime;
            talk.EndTime = expectedEndTime;
            talk.Room = expectedRoom;
            talk.Title = expectedTitle;
            talk.Speaker = expectedSpeaker;
            talk.Category = expectedCategory;

            // Assert
            Assert.Equal(expectedDay, talk.Day);
            Assert.Equal(expectedStartTime, talk.StartTime);
            Assert.Equal(expectedEndTime, talk.EndTime);
            Assert.Equal(expectedRoom, talk.Room);
            Assert.Equal(expectedTitle, talk.Title);
            Assert.Equal(expectedSpeaker, talk.Speaker);
            Assert.Equal(expectedCategory, talk.Category);
        }

        [Theory]
        [InlineData("Monday", "09:00", "10:00", "1", "Keynote", "Speaker A", "Talk")]
        [InlineData("Tuesday", "14:30", "15:30", "2", "Deep Dive", "Speaker B", "Workshop")]
        [InlineData("Friday", "16:00", "17:00", "3", "Panel Discussion", "Speaker C", "Panel")]
        public void Talk_Properties_HandlesVariousValidValues(string day, string startTime, string endTime, 
            string room, string title, string speaker, string category)
        {
            // Arrange
            var talk = new Talk();
            var expectedStartTime = TimeSpan.Parse(startTime);
            var expectedEndTime = TimeSpan.Parse(endTime);

            // Act
            talk.Day = day;
            talk.StartTime = expectedStartTime;
            talk.EndTime = expectedEndTime;
            talk.Room = room;
            talk.Title = title;
            talk.Speaker = speaker;
            talk.Category = category;

            // Assert
            Assert.Equal(day, talk.Day);
            Assert.Equal(expectedStartTime, talk.StartTime);
            Assert.Equal(expectedEndTime, talk.EndTime);
            Assert.Equal(room, talk.Room);
            Assert.Equal(title, talk.Title);
            Assert.Equal(speaker, talk.Speaker);
            Assert.Equal(category, talk.Category);
        }
    }
}