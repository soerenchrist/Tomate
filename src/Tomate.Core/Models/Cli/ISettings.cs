namespace Tomate.Models.Cli;

public interface ISettings
{
    public Minutes? FocusMinutes { get; set; }
    public Minutes? ShortBreakMinutes { get; set; }
    public Minutes? LongBreakMinutes { get; set; }
    public Count? LongBreakInterval { get; set; }
    public Cycles? Cycles { get; set; }
}