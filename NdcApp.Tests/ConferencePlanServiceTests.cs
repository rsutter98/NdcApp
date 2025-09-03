using Xunit;
using NdcApp.Core.Services;
using NdcApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NdcApp.Tests
{
    public class ConferencePlanServiceTests
    {
        private ConferencePlanService CreateService()
        {
            return new ConferencePlanService();
        }

        private Talk CreateTestTalk(string day = "Wednesday", string startTime = "09:00", string endTime = "10:00", 
            string room = "1", string title = "Test Talk", string speaker = "Test Speaker", string category = "Talk")
        {
            return new Talk
            {
                Day = day,
                StartTime = TimeSpan.Parse(startTime),
                EndTime = TimeSpan.Parse(endTime),
                Room = room,
                Title = title,
                Speaker = speaker,
                Category = category
            };
        }

        [Fact]
        public void SelectTalk_ValidTalk_AddsTalkToSelected()
        {
            // Arrange
            var service = CreateService();
            var talk = CreateTestTalk();

            // Act
            service.SelectTalk(talk);

            // Assert
            Assert.True(service.IsTalkSelected(talk));
            Assert.Single(service.GetSelectedTalks());
        }

        [Fact]
        public void SelectTalk_NullTalk_DoesNothing()
        {
            // Arrange
            var service = CreateService();

            // Act
            service.SelectTalk(null!);

            // Assert
            Assert.Empty(service.GetSelectedTalks());
        }

        [Fact]
        public void DeselectTalk_SelectedTalk_RemovesTalkFromSelected()
        {
            // Arrange
            var service = CreateService();
            var talk = CreateTestTalk();
            service.SelectTalk(talk);

            // Act
            service.DeselectTalk(talk);

            // Assert
            Assert.False(service.IsTalkSelected(talk));
            Assert.Empty(service.GetSelectedTalks());
        }

        [Fact]
        public void DeselectTalk_NullTalk_DoesNothing()
        {
            // Arrange
            var service = CreateService();
            var talk = CreateTestTalk();
            service.SelectTalk(talk);

            // Act
            service.DeselectTalk(null!);

            // Assert
            Assert.Single(service.GetSelectedTalks());
        }

        [Fact]
        public void DeselectTalk_NotSelectedTalk_DoesNothing()
        {
            // Arrange
            var service = CreateService();
            var talk = CreateTestTalk();

            // Act
            service.DeselectTalk(talk);

            // Assert
            Assert.Empty(service.GetSelectedTalks());
        }

        [Fact]
        public void IsTalkSelected_NullTalk_ReturnsFalse()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = service.IsTalkSelected(null!);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsTalkSelected_SameDayAndTime_DifferentTitle_ReturnsFalse()
        {
            // Arrange
            var service = CreateService();
            var talk1 = CreateTestTalk(title: "Talk 1");
            var talk2 = CreateTestTalk(title: "Talk 2"); // Same day/time, different title
            service.SelectTalk(talk1);

            // Act
            var result = service.IsTalkSelected(talk2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SelectTalk_SameDayAndTime_ReplacesExistingTalk()
        {
            // Arrange
            var service = CreateService();
            var talk1 = CreateTestTalk(title: "Talk 1");
            var talk2 = CreateTestTalk(title: "Talk 2"); // Same day/time, different title
            service.SelectTalk(talk1);

            // Act
            service.SelectTalk(talk2);

            // Assert
            Assert.False(service.IsTalkSelected(talk1));
            Assert.True(service.IsTalkSelected(talk2));
            Assert.Single(service.GetSelectedTalks());
        }

        [Fact]
        public void GetSelectedTalks_MultipleSelected_ReturnsAllSelected()
        {
            // Arrange
            var service = CreateService();
            var talk1 = CreateTestTalk(startTime: "09:00", title: "Talk 1");
            var talk2 = CreateTestTalk(startTime: "10:00", title: "Talk 2");
            var talk3 = CreateTestTalk(startTime: "11:00", title: "Talk 3");

            service.SelectTalk(talk1);
            service.SelectTalk(talk2);
            service.SelectTalk(talk3);

            // Act
            var selectedTalks = service.GetSelectedTalks();

            // Assert
            Assert.Equal(3, selectedTalks.Count);
            Assert.Contains(talk1.Title, selectedTalks.Select(t => t.Title));
            Assert.Contains(talk2.Title, selectedTalks.Select(t => t.Title));
            Assert.Contains(talk3.Title, selectedTalks.Select(t => t.Title));
        }

        [Fact]
        public void ClearSelectedTalks_WithSelectedTalks_RemovesAllSelected()
        {
            // Arrange
            var service = CreateService();
            var talk1 = CreateTestTalk(startTime: "09:00");
            var talk2 = CreateTestTalk(startTime: "10:00");
            service.SelectTalk(talk1);
            service.SelectTalk(talk2);

            // Act
            service.ClearSelectedTalks();

            // Assert
            Assert.Empty(service.GetSelectedTalks());
            Assert.False(service.IsTalkSelected(talk1));
            Assert.False(service.IsTalkSelected(talk2));
        }

        [Fact]
        public void SerializeSelectedTalks_NoSelected_ReturnsEmptyString()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = service.SerializeSelectedTalks();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void SerializeSelectedTalks_MultipleSelected_ReturnsCorrectFormat()
        {
            // Arrange
            var service = CreateService();
            var talk1 = CreateTestTalk(startTime: "09:00", title: "Talk 1");
            var talk2 = CreateTestTalk(startTime: "10:00", title: "Talk 2");
            service.SelectTalk(talk1);
            service.SelectTalk(talk2);

            // Act
            var result = service.SerializeSelectedTalks();

            // Assert
            Assert.Contains("Wednesday,09:00:00,10:00:00,1,Talk 1,Test Speaker,Talk", result);
            Assert.Contains("Wednesday,10:00:00,10:00:00,1,Talk 2,Test Speaker,Talk", result);
            Assert.Contains("|", result); // Should contain separator
        }

        [Fact]
        public void DeserializeSelectedTalks_ValidSerialized_RestoresSelectedTalks()
        {
            // Arrange
            var service = CreateService();
            var serialized = "Wednesday,09:00:00,10:00:00,1,Talk 1,Speaker 1,Talk|Thursday,14:00:00,15:00:00,2,Talk 2,Speaker 2,Workshop";

            // Act
            service.DeserializeSelectedTalks(serialized);

            // Assert
            var selectedTalks = service.GetSelectedTalks();
            Assert.Equal(2, selectedTalks.Count);
            
            var talk1 = selectedTalks.FirstOrDefault(t => t.Title == "Talk 1");
            var talk2 = selectedTalks.FirstOrDefault(t => t.Title == "Talk 2");
            
            Assert.NotNull(talk1);
            Assert.NotNull(talk2);
            Assert.Equal("Wednesday", talk1.Day);
            Assert.Equal("Thursday", talk2.Day);
        }

        [Fact]
        public void DeserializeSelectedTalks_EmptyString_ClearsSelected()
        {
            // Arrange
            var service = CreateService();
            service.SelectTalk(CreateTestTalk());

            // Act
            service.DeserializeSelectedTalks("");

            // Assert
            Assert.Empty(service.GetSelectedTalks());
        }

        [Fact]
        public void DeserializeSelectedTalks_NullString_ClearsSelected()
        {
            // Arrange
            var service = CreateService();
            service.SelectTalk(CreateTestTalk());

            // Act
            service.DeserializeSelectedTalks(null!);

            // Assert
            Assert.Empty(service.GetSelectedTalks());
        }

        [Fact]
        public void GetNextSelectedTalk_NoSelected_ReturnsNull()
        {
            // Arrange
            var service = CreateService();

            // Act
            var result = service.GetNextSelectedTalk();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetNextSelectedTalk_WithFutureTalks_ReturnsNextTalk()
        {
            // Arrange
            var service = CreateService();
            var now = DateTime.Now.TimeOfDay;
            var futureTalk1 = CreateTestTalk(startTime: now.Add(TimeSpan.FromHours(1)).ToString(@"hh\:mm"), title: "Future Talk 1");
            var futureTalk2 = CreateTestTalk(startTime: now.Add(TimeSpan.FromHours(2)).ToString(@"hh\:mm"), title: "Future Talk 2");
            
            service.SelectTalk(futureTalk2); // Add second talk first
            service.SelectTalk(futureTalk1); // Add first talk second

            // Act
            var result = service.GetNextSelectedTalk();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Future Talk 1", result.Title); // Should return the earlier future talk
        }

        [Fact]
        public void SortTalksBySpeaker_UnsortedList_ReturnsSortedBySpeaker()
        {
            // Arrange
            var service = CreateService();
            var talks = new List<Talk>
            {
                CreateTestTalk(speaker: "Charlie"),
                CreateTestTalk(speaker: "Alice"),
                CreateTestTalk(speaker: "Bob")
            };

            // Act
            var result = service.SortTalksBySpeaker(talks);

            // Assert
            Assert.Equal("Alice", result[0].Speaker);
            Assert.Equal("Bob", result[1].Speaker);
            Assert.Equal("Charlie", result[2].Speaker);
        }

        [Fact]
        public void SortTalksByCategory_UnsortedList_ReturnsSortedByCategory()
        {
            // Arrange
            var service = CreateService();
            var talks = new List<Talk>
            {
                CreateTestTalk(category: "Workshop"),
                CreateTestTalk(category: "Keynote"),
                CreateTestTalk(category: "Talk")
            };

            // Act
            var result = service.SortTalksByCategory(talks);

            // Assert
            Assert.Equal("Keynote", result[0].Category);
            Assert.Equal("Talk", result[1].Category);
            Assert.Equal("Workshop", result[2].Category);
        }

        [Fact]
        public void SortTalksStandard_UnsortedList_ReturnsSortedByDayAndTime()
        {
            // Arrange
            var service = CreateService();
            var talks = new List<Talk>
            {
                CreateTestTalk(day: "Thursday", startTime: "10:00"),
                CreateTestTalk(day: "Wednesday", startTime: "14:00"),
                CreateTestTalk(day: "Wednesday", startTime: "09:00")
            };

            // Act
            var result = service.SortTalksStandard(talks);

            // Assert
            Assert.Equal("Wednesday", result[0].Day);
            Assert.Equal(TimeSpan.Parse("09:00"), result[0].StartTime);
            Assert.Equal("Wednesday", result[1].Day);
            Assert.Equal(TimeSpan.Parse("14:00"), result[1].StartTime);
            Assert.Equal("Thursday", result[2].Day);
        }

        [Fact]
        public void SerializeAndDeserialize_RoundTrip_PreservesData()
        {
            // Arrange
            var service = CreateService();
            var originalTalk = CreateTestTalk(day: "Friday", startTime: "16:30", title: "Original Talk", speaker: "Original Speaker");
            service.SelectTalk(originalTalk);

            // Act
            var serialized = service.SerializeSelectedTalks();
            var newService = CreateService();
            newService.DeserializeSelectedTalks(serialized);

            // Assert
            var restoredTalks = newService.GetSelectedTalks();
            Assert.Single(restoredTalks);
            var restoredTalk = restoredTalks[0];
            Assert.Equal(originalTalk.Day, restoredTalk.Day);
            Assert.Equal(originalTalk.StartTime, restoredTalk.StartTime);
            Assert.Equal(originalTalk.Title, restoredTalk.Title);
            Assert.Equal(originalTalk.Speaker, restoredTalk.Speaker);
        }
    }
}