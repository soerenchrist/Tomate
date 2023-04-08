namespace Tomate.Models;

public record struct Settings
{
    public Minutes FocusMinutes { get; init; } = 25;
    public Minutes ShortBreakMinutes { get; init; } = 5;
    public Minutes LongBreakMinutes { get; init; } = 15;
    public Count LongBreakInterval { get; init; } = 4;
    public Cycles Cycles { get; init; } = Cycles.Infinite;

    public Settings()
    {
        FocusMinutes = 25;
        ShortBreakMinutes = 5;
        LongBreakMinutes = 15;
        LongBreakInterval = 4;
        Cycles = Cycles.Infinite;
    }
}