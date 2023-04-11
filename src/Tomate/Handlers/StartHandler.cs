using Tomate.Models;
using Tomate.Models.Cli;
using Tomate.Services;
using Tomate.Services.Abstractions;

namespace Tomate.Handlers;

public class StartHandler : IAsyncHandler<StartArgs>
{
    private readonly ISettingsService _settingsService;
    private readonly IScheduler _scheduler;
    private readonly INotifyService _notifyService;

    public StartHandler(ISettingsService settingsService,
        IScheduler scheduler,
        INotifyService notifyService)
    {
        _settingsService = settingsService;
        _scheduler = scheduler;
        _notifyService = notifyService;
    }

    public async Task<int> HandleAsync(StartArgs args, CancellationToken cancellationToken = default)
    {
        var settings = GetMergedSettings(args);

        if (settings.Cycles == Cycles.Infinite)
        {
            await HandleIndefinite(settings, cancellationToken);
        }
        else
        {
            await HandleDefinite(settings, cancellationToken);
        }

        return 0;
    }

    private async Task HandleIndefinite(Settings settings, CancellationToken cancellationToken)
    {
        while (true)
        {
            await HandleSingleCycle(settings, cancellationToken);
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private async Task HandleDefinite(Settings settings, CancellationToken cancellationToken)
    {
        for (var cycle = 0; cycle < settings.Cycles; cycle++)
        {
            await _notifyService.NotifyStartOfCycle(cycle, settings.Cycles);
            await HandleSingleCycle(settings, cancellationToken);
        }
    }

    private async Task HandleSingleCycle(Settings settings, CancellationToken cancellationToken)
    {
        for (var interval = 0; interval < settings.LongBreakInterval; interval++)
        {
            await PerformFocusTime(settings, cancellationToken);
            await PerformShortBreak(settings, cancellationToken);
        }

        await PerformFocusTime(settings, cancellationToken);
        await PerformLongBreak(settings, cancellationToken);
    }

    private async Task PerformFocusTime(Settings settings, CancellationToken cancellationToken)
    {
        await _notifyService.NotifyStartOfFocusTime(settings.FocusMinutes);
        var remainingMinutes = _scheduler.ScheduleMinutes(settings.FocusMinutes, cancellationToken);

        await foreach (var remainingMinute in remainingMinutes.WithCancellation(cancellationToken))
        {
            await _notifyService.NotifyRemainingFocusTime(remainingMinute);
        }
    }

    private async Task PerformShortBreak(Settings settings, CancellationToken cancellationToken)
    {
        await _notifyService.NotifyStartOfBreakTime(settings.ShortBreakMinutes);
        var remainingMinutes = _scheduler.ScheduleMinutes(settings.ShortBreakMinutes, cancellationToken);

        await foreach (var remainingMinute in remainingMinutes.WithCancellation(cancellationToken))
        {
            await _notifyService.NotifyRemainingBreakTime(remainingMinute);
        }
    }

    private async Task PerformLongBreak(Settings settings, CancellationToken cancellationToken)
    {
        await _notifyService.NotifyStartOfBreakTime(settings.LongBreakMinutes);
        var remainingMinutes = _scheduler.ScheduleMinutes(settings.LongBreakMinutes, cancellationToken);

        await foreach (var remainingMinute in remainingMinutes.WithCancellation(cancellationToken))
        {
            await _notifyService.NotifyRemainingBreakTime(remainingMinute);
        }
    }

    private Settings GetMergedSettings(StartArgs args)
    {
        var globalSettings = _settingsService.ReadGlobalSettings();
        var mergedSettings = new SettingsMerger().MergeGlobalAndLocalSettings(globalSettings, args);
        return mergedSettings;
    }
}