using System.ComponentModel;
using System.Text.Json.Serialization;
using Tomate.Converters;

namespace Tomate.Models;

[TypeConverter(typeof(CountTypeConverter))]
public struct Count
{
    public int Value { get; }

    [JsonConstructor]
    public Count(int value)
    {
        if (value < 1)
            throw new ArgumentOutOfRangeException(nameof(value),
                "Count must be greater than or equal to one.");
        Value = value;
    }

    public static implicit operator Count(int value) => new Count(value);
    public static implicit operator int(Count count) => count.Value;

    public override string ToString() => $"{Value.ToString()} times";

    public override bool Equals(object? obj)
    {
        if (obj is Count count)
        {
            return Value == count.Value;
        }

        return false;
    }

    public bool Equals(Count other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value;
    }
}