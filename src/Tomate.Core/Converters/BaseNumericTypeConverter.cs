using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Tomate.Converters;

public abstract class BaseNumericTypeConverter<T> : TypeConverter
    where T : struct
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        if (sourceType == typeof(string))
        {
            return true;
        }
        return base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
    {
        if (destinationType is not null && destinationType == typeof(T))
        {
            return true;
        }
        return base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string stringValue)
        {
            if (int.TryParse(stringValue, out var intValue))
            {
                try
                {
                    return Create(intValue);
                }
                catch (ArgumentOutOfRangeException)
                {
                    
                }
            }
        }
        throw new FormatException("Could not convert string to type.");
    }

    public abstract T Create(int value);
}