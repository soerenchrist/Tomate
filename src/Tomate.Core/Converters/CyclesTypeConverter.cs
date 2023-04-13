using Tomate.Models;

namespace Tomate.Converters;

public class CyclesTypeConverter : BaseNumericTypeConverter<Cycles>
{
    public override Cycles Create(int value) => new Cycles(value);
}