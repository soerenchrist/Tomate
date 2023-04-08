using Tomate.Models;
using Tomate.Models.Cli;

namespace Tomate.Services;

public class SettingsMerger
{
    public Settings MergeGlobalAndLocalSettings(Settings globalSettings, StartArgs startArgs)
    {
        var mergedSettings = new Settings
        {
            FocusMinutes = startArgs.FocusMinutes ?? globalSettings.FocusMinutes,
            ShortBreakMinutes = startArgs.ShortBreakMinutes ?? globalSettings.ShortBreakMinutes,
            LongBreakMinutes = startArgs.LongBreakMinutes ?? globalSettings.LongBreakMinutes,
            LongBreakInterval = startArgs.LongBreakInterval ?? globalSettings.LongBreakInterval,
        };
        return mergedSettings;
    }
    
}