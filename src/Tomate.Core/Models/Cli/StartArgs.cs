using CommandLine;

namespace Tomate.Models.Cli;

[Verb("start", HelpText = "Start a new pomodoro session")]
public class StartArgs : ISettings
{
    [Option('f', "focus-minutes", Required = false, HelpText = "Set the length of your focus time in minutes")]
    public Minutes? FocusMinutes { get; set; }

    [Option('s', "short-break-minutes", Required = false, HelpText = "Set the length of your short break in minutes")]
    public Minutes? ShortBreakMinutes { get; set; }

    [Option('l', "long-break-minutes", Required = false, HelpText = "Set the length of your long break in minutes")]
    public Minutes? LongBreakMinutes { get; set; }

    [Option('i', "long-break-interval", Required = false,
        HelpText = "Set the number of focus sessions before a long break")]
    public Count? LongBreakInterval { get; set; }

    [Option('c', "cycles", Required = false, HelpText = "Set the amount of cycles to run")]
    public Cycles? Cycles { get; set; }
}