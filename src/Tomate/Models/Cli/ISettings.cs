namespace Tomate.Models.Cli;

public interface ISettings
{
    public int? FocusMinutes { get; set; }
    public int? ShortBreakMinutes { get; set; }
    public int? LongBreakMinutes { get; set; }
    public int? LongBreakInterval { get; set; }
    public int? Cycles { get; set; }
}