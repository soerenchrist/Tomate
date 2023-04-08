using NSubstitute;
using Tomate.Handlers;
using Tomate.Models;
using Tomate.Models.Cli;
using Tomate.Services;
using Tomate.Services.Abstractions;
using Tomate.Tests.Utils;

namespace Tomate.Tests.Handlers;

public class StartHandlerTests
{
    private readonly StartHandler cut;
    private readonly ISettingsService settingsService = Substitute.For<ISettingsService>();
    private readonly INotifyService notifyService = Substitute.For<INotifyService>();
    private readonly CancellationTokenSource cts;

    public StartHandlerTests()
    {
        this.cts = new CancellationTokenSource();
        cut = new StartHandler(settingsService, new TestScheduler(), notifyService);

        settingsService.ReadGlobalSettings().Returns(new Settings
        {
            Cycles = 1,
            LongBreakInterval = 1
        });
    }

    [Fact]
    public async Task HandleAsync_WithDefaultSettings_ShouldNotifyFocusTime25Times()
    {
        await cut.HandleAsync(new StartArgs(), cts.Token);

        notifyService.Received(50).NotifyRemainingFocusTime(Arg.Any<Minutes>());
    }

    [Fact]
    public async Task HandleAsync_WithPassedSettings_ShouldNotifyFocusTimeCorrespondingTimes()
    {
        await cut.HandleAsync(new StartArgs
        {
            FocusMinutes = 10
        }, cts.Token);
        notifyService.Received(20).NotifyRemainingFocusTime(Arg.Any<Minutes>());
    }

    [Fact]
    public async Task HandleAsync_RunsCorrectAmountOfFocusTimes()
    {
        var focusMinutes = 25;
        var intervalsPerCycle = 4;
        var expectedAmountOfFocusTimes = intervalsPerCycle * focusMinutes + focusMinutes;
        await cut.HandleAsync(new StartArgs
        {
            LongBreakInterval = intervalsPerCycle
        }, cts.Token);
        notifyService.Received(expectedAmountOfFocusTimes).NotifyRemainingFocusTime(Arg.Any<Minutes>());
    }

    [Fact]
    public async Task HandleAsync_RunsCorrectAmountOfBreakTimes()
    {
        var shortBreakMinutes = 5;
        var longBreakMinutes = 30;
        var intervalsPerCycle = 4;
        var expectedAmountOfBreakTimes = (shortBreakMinutes * intervalsPerCycle) + longBreakMinutes;
        await cut.HandleAsync(new StartArgs
        {
            ShortBreakMinutes = shortBreakMinutes,
            LongBreakMinutes = longBreakMinutes,
            LongBreakInterval = intervalsPerCycle
        }, cts.Token);
        notifyService.Received(expectedAmountOfBreakTimes).NotifyRemainingBreakTime(Arg.Any<Minutes>());
    }

    [Fact]
    public async Task HandleAsync_WhenCacnelled_ShouldStop()
    {
        cts.CancelAfter(100);
        try
        {
            await cut.HandleAsync(new StartArgs
            {
                Cycles = Cycles.Infinite
            }, cts.Token);
        }
        catch (TaskCanceledException)
        {
        }
    }
}