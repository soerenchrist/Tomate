using CommandLine;

namespace Tomate.Models.Cli;

[Verb("start", HelpText = "Start a new pomodoro session")]
public class StartArgs
{
    [Option('f', "focus-minutes", Required = false, HelpText = "Set the length of your focus time in minutes")]
    public int? FocusMinutes { get; set; }

    [Option('s', "short-break-minutes", Required = false, HelpText = "Set the length of your short break in minutes")]
    public int? ShortBreakMinutes { get; set; }

    [Option('l', "long-break-minutes", Required = false, HelpText = "Set the length of your long break in minutes")]
    public int? LongBreakMinutes { get; set; }

    [Option('i', "long-break-interval", Required = false,
        HelpText = "Set the number of focus sessions before a long break")]
    public int? LongBreakInterval { get; set; }
}