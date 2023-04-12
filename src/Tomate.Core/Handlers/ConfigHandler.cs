using Tomate.Models;
using Tomate.Models.Cli;
using Tomate.Services;
using Tomate.Services.Abstractions;

namespace Tomate.Handlers;

public class ConfigHandler : IAsyncHandler<ConfigArgs>
{
    private readonly ISettingsService _settingsService;
    private readonly IOutput _output;

    public ConfigHandler(ISettingsService settingsService, IOutput output)
    {
        _settingsService = settingsService;
        _output = output;
    }
    
    public Task<int> HandleAsync(ConfigArgs args, CancellationToken cancellationToken = default)
    {
        if (IsEmpty(args))
        {
            _output.WriteErrorLine("No arguments provided. Config was not changed. Use --help to see available options.");
            return Task.FromResult(1); 
        }

        var settings = MergeSettings(args);
        _settingsService.UpdateGlobalSettings(settings);

        return Task.FromResult(0);
    }

    private Settings MergeSettings(ConfigArgs args)
    {
        var globalSettings = _settingsService.ReadGlobalSettings();
        var mergedSettings = new SettingsMerger().MergeGlobalAndLocalSettings(globalSettings, args);

        return mergedSettings;
    }

    private bool IsEmpty(ConfigArgs args)
    {
        return args is
        {
            Cycles: null,
            FocusMinutes: null,
            LongBreakInterval: null,
            LongBreakMinutes: null,
            ShortBreakMinutes: null
        };
    }
}