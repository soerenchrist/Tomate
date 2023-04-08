namespace Tomate.Models;

public class Settings
{
    public Minutes FocusMinutes { get; set; } = 25;
    public Minutes ShortBreakMinutes { get; set; } = 5;
    public Minutes LongBreakMinutes { get; set; } = 15;
    public int LongBreakInterval { get; set; } = 4;
}