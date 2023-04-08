using System;
using System.Text.Json;
using Tomate.Models;

namespace Tomate.Tests.Models;

public class CyclesTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Cycles_ThrowsArgumentOutOfRangeException_WhenNumberOfMinutesIsLessThanZero(int value)
    {
        var action = () => new Cycles(value);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void Cycles_WhenPassedValidValues_ShouldNotThrow(int value)
    {
        var action = () => new Cycles(value);

        action.Should().NotThrow<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Cycles_CanBeImplicitlyConvertedFromInt()
    {
        Cycles count = 25;
        count.Value.Should().Be(25);
    }

    [Fact]
    public void Cycles_CanBeImplicitlyConvertedToInt()
    {
        int count = new Cycles(25);
        count.Should().Be(25);
    }

    [Fact]
    public void Cycles_OverridesToString()
    {
        var count = new Cycles(25);
        count.ToString().Should().Be("25 times");
    }

    [Fact]
    public void Cycles_OverridesEquals()
    {
        var count = new Cycles(25);
        var otherCount = new Cycles(25);
        count.Equals(otherCount).Should().BeTrue();
    }

    [Fact]
    public void Cycles_OverridesEqualsSign()
    {
        var count = new Cycles(25);
        var otherCount = new Cycles(25);
        (count == otherCount).Should().BeTrue();
    }

    [Fact]
    public void Cycles_CanBeDeserializedFromJson()
    {
        var json = """
        {
            "Value": 25
        }
        """;

        var count = JsonSerializer.Deserialize<Cycles>(json);
        count.Should().BeEquivalentTo(new Cycles(25));
    }
}