using System;
using System.ComponentModel;
using System.Text.Json;
using Tomate.Models;

namespace Tomate.Tests.Models;

public class CountTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Count_ThrowsArgumentOutOfRangeException_WhenNumberOfMinutesIsLessThanZero(int value)
    {
        var action = () => new Count(value);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void Count_WhenPassedValidValues_ShouldNotThrow(int value)
    {
        var action = () => new Count(value);

        action.Should().NotThrow<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Count_CanBeImplicitlyConvertedFromInt()
    {
        Count count = 25;
        count.Value.Should().Be(25);
    }

    [Fact]
    public void Count_CanBeImplicitlyConvertedToInt()
    {
        int count = new Count(25);
        count.Should().Be(25);
    }

    [Fact]
    public void Count_OverridesToString()
    {
        var count = new Count(25);
        count.ToString().Should().Be("25 times");
    }

    [Fact]
    public void Count_OverridesEquals()
    {
        var count = new Count(25);
        var otherCount = new Count(25);
        count.Equals(otherCount).Should().BeTrue();
    }

    [Fact]
    public void Count_OverridesEqualsSign()
    {
        var count = new Count(25);
        var otherCount = new Count(25);
        (count == otherCount).Should().BeTrue();
    }

    [Fact]
    public void Count_CanBeDeserializedFromJson()
    {
        var json = """
        {
            "Value": 25
        }
        """;

        var count = JsonSerializer.Deserialize<Count>(json);
        count.Should().BeEquivalentTo(new Count(25));
    }

    [Fact]
    public void Count_ShouldHaveTypeConverterAssociated()
    {
        TypeDescriptor.GetConverter(typeof(Count)).Should().NotBeNull();
    }
}