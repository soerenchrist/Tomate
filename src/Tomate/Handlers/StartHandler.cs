using Tomate.Models;
using Tomate.Models.Cli;
using Tomate.Services;

namespace Tomate.Handlers;

public class StartHandler : IHandler<StartArgs>
{
    private readonly SettingsService _settingsService;

    public StartHandler(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public int Handle(StartArgs args)
    {
        var settings = GetMergedSettings(args);
        return 0;
    }

    private Settings GetMergedSettings(StartArgs args)
    {
        var globalSettings = _settingsService.ReadGlobalSettings();
        var mergedSettings = new SettingsMerger().MergeGlobalAndLocalSettings(globalSettings, args);
        return mergedSettings;
    }
}