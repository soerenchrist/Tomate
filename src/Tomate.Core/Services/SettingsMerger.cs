using Tomate.Models;
using Tomate.Models.Cli;

namespace Tomate.Services;

public class SettingsMerger
{
    public Settings MergeGlobalAndLocalSettings(Settings globalSettings, ISettings args)
    {
        var mergedSettings = new Settings
        {
            FocusMinutes = args.FocusMinutes ?? globalSettings.FocusMinutes,
            ShortBreakMinutes = args.ShortBreakMinutes ?? globalSettings.ShortBreakMinutes,
            LongBreakMinutes = args.LongBreakMinutes ?? globalSettings.LongBreakMinutes,
            LongBreakInterval = args.LongBreakInterval ?? globalSettings.LongBreakInterval,
            Cycles = args.Cycles ?? globalSettings.Cycles
        };
        return mergedSettings;
    }
    
}