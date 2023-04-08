using System.Text.Json;
using Tomate.Models;

namespace Tomate.Tests.Models;

public class MinutesTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-10)]
    public void Minutes_ThrowsArgumentOutOfRangeException_WhenNumberOfMinutesIsLessThanZero(int value)
    {
        var action = () => new Minutes(value);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void Minutes_WhenValidValuesArePassed_ShouldNotThrow(int value)
    {
        var action = () => new Minutes(value);

        action.Should().NotThrow<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Minutes_CanBeImplicitlyConvertedFromInt()
    {
        Minutes minutes = 25;
        minutes.Value.Should().Be(25);
    }

    [Fact]
    public void Minutes_CanBeImplicitlyConvertedToInt()
    {
        int minutes = new Minutes(25);
        minutes.Should().Be(25);
    }

    [Fact]
    public void Minutes_OverridesToString()
    {
        var minutes = new Minutes(25);
        minutes.ToString().Should().Be("25min");
    }

    [Fact]
    public void Minutes_OverridesEquals()
    {
        var minutes = new Minutes(25);
        var otherMinutes = new Minutes(25);
        minutes.Equals(otherMinutes).Should().BeTrue();
    }

    [Fact]
    public void Minutes_OverridesEqualsSign()
    {
        var minutes = new Minutes(25);
        var otherMinutes = new Minutes(25);
        (minutes == otherMinutes).Should().BeTrue();
    }

    [Fact]
    public void Minutes_CanBeDeserializedFromJson()
    {
        var json = """
        {
            "Value": 25
        }
        """;
        
        var minutes = JsonSerializer.Deserialize<Minutes>(json);
        minutes.Should().BeEquivalentTo(new Minutes(25));
    }
}