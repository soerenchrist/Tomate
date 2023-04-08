namespace Tomate.Models;

public struct Count
{
    public int Value { get; }

    public Count(int count)
    {
        if (count < 1)
            throw new ArgumentOutOfRangeException(nameof(count),
                "Count must be greater than or equal to one.");
        Value = count;
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

    public bool Equals(Minutes other)
    {
        return Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value;
    }
}