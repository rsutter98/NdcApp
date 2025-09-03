using Xunit;
using NdcApp.Core.Services;
using NdcApp.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NdcApp.Tests
{
    public class TalkServiceTests
    {
        private readonly string testCsvContent = @"Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie
Wednesday,09:00,10:00,1,Keynote: AI is having its moment ... again,Jodie Burchell,Talk
Wednesday,10:20,11:20,1,Java Sucks (So C# Didn't Have To),Adele Carpenter,Talk
Wednesday,10:20,11:20,2,Navigating complexity in event-driven architectures,David Boyne,Talk
Thursday,14:00,15:00,3,The future & challenges of cloud,Anders Lybecker,Workshop";

        [Fact]
        public void LoadTalks_FileNotFound_ThrowsFileNotFoundException()
        {
            // Arrange
            var nonExistentPath = "/path/that/does/not/exist.csv";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => TalkService.LoadTalks(nonExistentPath));
        }

        [Fact]
        public void LoadTalks_ValidCsvFile_ReturnsParsedTalks()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, testCsvContent);

            try
            {
                // Act
                var talks = TalkService.LoadTalks(tempFile);

                // Assert
                Assert.Equal(4, talks.Count);
                
                var firstTalk = talks[0];
                Assert.Equal("Wednesday", firstTalk.Day);
                Assert.Equal(new TimeSpan(9, 0, 0), firstTalk.StartTime);
                Assert.Equal(new TimeSpan(10, 0, 0), firstTalk.EndTime);
                Assert.Equal("1", firstTalk.Room);
                Assert.Equal("Keynote: AI is having its moment ... again", firstTalk.Title);
                Assert.Equal("Jodie Burchell", firstTalk.Speaker);
                Assert.Equal("Talk", firstTalk.Category);
            }
            finally
            {
                File.Delete(tempFile);
            }
        }

        [Fact]
        public void ParseTalksFromString_ValidCsvContent_ReturnsParsedTalks()
        {
            // Act
            var talks = TalkService.ParseTalksFromString(testCsvContent);

            // Assert
            Assert.Equal(4, talks.Count);
            
            var lastTalk = talks[3];
            Assert.Equal("Thursday", lastTalk.Day);
            Assert.Equal(new TimeSpan(14, 0, 0), lastTalk.StartTime);
            Assert.Equal(new TimeSpan(15, 0, 0), lastTalk.EndTime);
            Assert.Equal("3", lastTalk.Room);
            Assert.Equal("The future & challenges of cloud", lastTalk.Title);
            Assert.Equal("Anders Lybecker", lastTalk.Speaker);
            Assert.Equal("Workshop", lastTalk.Category);
        }

        [Fact]
        public void ParseTalksFromString_EmptyContent_ReturnsEmptyList()
        {
            // Act
            var talks = TalkService.ParseTalksFromString("");

            // Assert
            Assert.Empty(talks);
        }

        [Fact]
        public void ParseTalksFromString_OnlyHeader_ReturnsEmptyList()
        {
            // Arrange
            var headerOnly = "Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie";

            // Act
            var talks = TalkService.ParseTalksFromString(headerOnly);

            // Assert
            Assert.Empty(talks);
        }

        [Fact]
        public void ParseTalksFromString_InvalidLines_SkipsInvalidLines()
        {
            // Arrange
            var invalidCsv = @"Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie
Wednesday,09:00,10:00,1,Valid Talk,Speaker A,Talk
InvalidLine
Wednesday,invalid_time,11:00,2,Another Valid Talk,Speaker B,Talk
TooFewColumns,Only,Three";

            // Act
            var talks = TalkService.ParseTalksFromString(invalidCsv);

            // Assert
            Assert.Single(talks); // Only one valid talk should be parsed
            Assert.Equal("Valid Talk", talks[0].Title);
        }

        [Fact]
        public void ParseTalksFromString_AllValidData_ParsesCorrectly()
        {
            // Act
            var talks = TalkService.ParseTalksFromString(testCsvContent);

            // Assert
            Assert.All(talks, talk =>
            {
                Assert.False(string.IsNullOrEmpty(talk.Day));
                Assert.False(string.IsNullOrEmpty(talk.Room));
                Assert.False(string.IsNullOrEmpty(talk.Title));
                Assert.False(string.IsNullOrEmpty(talk.Speaker));
                Assert.False(string.IsNullOrEmpty(talk.Category));
                Assert.True(talk.StartTime < talk.EndTime);
            });
        }

        [Theory]
        [InlineData("Wednesday,09:00,10:00,1,Test Talk,Test Speaker,Talk")]
        [InlineData("Thursday,14:30,15:30,2,Another Talk,Another Speaker,Workshop")]
        public void ParseTalksFromString_SingleValidLine_ParsesCorrectly(string csvLine)
        {
            // Arrange
            var csvWithHeader = $"Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie\n{csvLine}";

            // Act
            var talks = TalkService.ParseTalksFromString(csvWithHeader);

            // Assert
            Assert.Single(talks);
            var talk = talks[0];
            var parts = csvLine.Split(',');
            Assert.Equal(parts[0], talk.Day);
            Assert.Equal(TimeSpan.Parse(parts[1]), talk.StartTime);
            Assert.Equal(TimeSpan.Parse(parts[2]), talk.EndTime);
            Assert.Equal(parts[3], talk.Room);
            Assert.Equal(parts[4], talk.Title);
            Assert.Equal(parts[5], talk.Speaker);
            Assert.Equal(parts[6], talk.Category);
        }
    }
}