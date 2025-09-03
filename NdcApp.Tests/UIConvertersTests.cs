using Xunit;
using NdcApp.Core.Converters;

namespace NdcApp.Tests
{
    public class UIConvertersTests
    {
        [Fact]
        public void InverseBoolConverter_True_ReturnsFalse()
        {
            // Arrange
            var converter = new InverseBoolConverter();

            // Act
            var result = converter.Convert(true);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void InverseBoolConverter_False_ReturnsTrue()
        {
            // Arrange
            var converter = new InverseBoolConverter();

            // Act
            var result = converter.Convert(false);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(true, "Selected")]
        [InlineData(false, "Select")]
        public void SelectedTextConverter_BoolValue_ReturnsCorrectText(bool selected, string expectedText)
        {
            // Arrange
            var converter = new SelectedTextConverter();

            // Act
            var result = converter.Convert(selected);

            // Assert
            Assert.Equal(expectedText, result);
        }

        [Theory]
        [InlineData(true, "#FFB400")]
        [InlineData(false, "#0A2342")]
        public void SelectedColorConverter_BoolValue_ReturnsCorrectColor(bool selected, string expectedColor)
        {
            // Arrange
            var converter = new SelectedColorConverter();

            // Act
            var result = converter.Convert(selected);

            // Assert
            Assert.Equal(expectedColor, result);
        }

        [Fact]
        public void SelectedTextConverter_SelectedTrue_ReturnsSelected()
        {
            // Arrange
            var converter = new SelectedTextConverter();

            // Act
            var result = converter.Convert(true);

            // Assert
            Assert.Equal("Selected", result);
        }

        [Fact]
        public void SelectedTextConverter_SelectedFalse_ReturnsSelect()
        {
            // Arrange
            var converter = new SelectedTextConverter();

            // Act
            var result = converter.Convert(false);

            // Assert
            Assert.Equal("Select", result);
        }

        [Fact]
        public void SelectedColorConverter_SelectedTrue_ReturnsOrangeColor()
        {
            // Arrange
            var converter = new SelectedColorConverter();

            // Act
            var result = converter.Convert(true);

            // Assert
            Assert.Equal("#FFB400", result);
        }

        [Fact]
        public void SelectedColorConverter_SelectedFalse_ReturnsBlueColor()
        {
            // Arrange
            var converter = new SelectedColorConverter();

            // Act
            var result = converter.Convert(false);

            // Assert
            Assert.Equal("#0A2342", result);
        }
    }
}