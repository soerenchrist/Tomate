using Tomate.Models;

namespace Tomate.Converters;

public class CountTypeConverter : BaseNumericTypeConverter<Count>
{
    public override Count Create(int value) => new Count(value);
}