using System.ComponentModel;
using System.Text.Json.Serialization;
using Tomate.Converters;

namespace Tomate.Models;

[TypeConverter(typeof(CyclesTypeConverter))]
public struct Cycles
{
    public static Cycles Infinite = new Cycles(0);
    public int Value { get; }

    [JsonConstructor]
    public Cycles(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value),
                "Cycles must be greater than or equal to 0.");
        Value = value;
    }

    public static implicit operator Cycles(int value) => new Cycles(value);
    public static implicit operator int(Cycles count) => count.Value;

    public override string ToString() => $"{Value.ToString()} times";

    public override bool Equals(object? obj)
    {
        if (obj is Cycles count)
        {
            return Value == count.Value;
        }

        return false;
    }

    public bool Equals(Cycles other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value;
    }
}