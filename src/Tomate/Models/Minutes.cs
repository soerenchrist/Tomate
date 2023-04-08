namespace Tomate.Models;

public struct Minutes
{
    public int Value { get; }

    public Minutes(int numberOfMinutes)
    {
        if (numberOfMinutes < 1)
            throw new ArgumentOutOfRangeException(nameof(numberOfMinutes),
                "Number of minutes must be greater than or equal to one.");
        Value = numberOfMinutes;
    }

    public static implicit operator Minutes(int numberOfMinutes) => new Minutes(numberOfMinutes);
    public static implicit operator int(Minutes minutes) => minutes.Value;

    public override string ToString() => $"{Value.ToString()}min";

    public override bool Equals(object? obj)
    {
        if (obj is Minutes minutes)
        {
            return Value == minutes.Value;
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