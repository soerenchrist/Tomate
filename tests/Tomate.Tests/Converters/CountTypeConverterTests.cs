using Tomate.Converters;
using Tomate.Models;

namespace Tomate.Tests.Converters;

public class CountTypeConverterTests
{
    private readonly CountTypeConverter converter;
    public CountTypeConverterTests()
    {
        this.converter = new CountTypeConverter();
    }

    [Fact]
    public void CanConvertFrom_ShouldReturnTrue_WhenStringIsPassed()
    {
        var result = converter.CanConvertFrom(typeof(string));
        result.Should().BeTrue();
    }

    [Fact]
    public void CanConvertFrom_ShouldReturnFalse_WhenAnythingElseIsPassed()
    {
        var result = converter.CanConvertFrom(typeof(bool));
        result.Should().BeFalse();
    }
    [Fact]
    public void CanConvertTo_ShouldReturnTrue_WhenMinutesIsPassed()
    {
        var result = converter.CanConvertTo(typeof(Count));
        result.Should().BeTrue();
    }

    [Fact]
    public void CanConvertTo_ShouldReturnFalse_WhenAnythingElseIsPassed()
    {
        var result = converter.CanConvertTo(typeof(bool));
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("2147483647", int.MaxValue)]
    public void Convert_ShouldSuccessfullyConvert_WhenValidStringIsPassed(string input, int expectedOutput)
    {
        var result = converter.ConvertFrom(input);
        result.Should().BeOfType<Count>();
        result.Should().Be(new Count(expectedOutput));
    }

    [Theory]
    [InlineData("0")]
    [InlineData("-1")]
    [InlineData("abc")]
    [InlineData("123.123")]
    [InlineData("123,123")]
    public void Convert_ShouldThrow_WhenInValidStringIsPassed(string input)
    {
        var action = () => converter.ConvertFrom(input);
        action.Should().Throw<FormatException>();
    }
}