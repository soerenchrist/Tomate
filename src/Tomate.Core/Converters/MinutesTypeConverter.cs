using Tomate.Models;

namespace Tomate.Converters;

public class MinutesTypeConverter : BaseNumericTypeConverter<Minutes>
{
    public override Minutes Create(int value) => new Minutes(value);
}