using Tomate.Models;
using Tomate.Models.Cli;
using Tomate.Services;

namespace Tomate.Tests.Services;

public class SettingsMergerTests
{
    private readonly SettingsMerger cut;

    public SettingsMergerTests()
    {
        this.cut = new SettingsMerger();
    }

    [Fact]
    public void MergeGlobalAndLocalSettings_WhenNoLocalSettingsAreProvided_ShouldReturnGlobalSettings()
    {
        var globalSettings = new Settings
        {
            FocusMinutes = 10,
            LongBreakInterval = 3,
            LongBreakMinutes = 20,
            ShortBreakMinutes = 4,
            Cycles = Cycles.Infinite
        };
        var startArgs = new StartArgs();

        var mergedSettings = this.cut.MergeGlobalAndLocalSettings(globalSettings, startArgs);

        mergedSettings.Should().BeEquivalentTo(globalSettings);
    }

    [Fact]
    public void MergeGlobalAndLocalSettings_WhenSomeLocalValuesAreProvided_ShouldReturnMergedState()
    {
        var globalSettings = new Settings
        {
            FocusMinutes = 10,
            LongBreakInterval = 3,
            LongBreakMinutes = 20,
            ShortBreakMinutes = 4,
            Cycles = Cycles.Infinite
        };
        var startArgs = new StartArgs
        {
            FocusMinutes = 20
        };

        var mergedSettings = this.cut.MergeGlobalAndLocalSettings(globalSettings, startArgs);

        mergedSettings.Should().BeEquivalentTo(new Settings
        {
            FocusMinutes = 20,
            LongBreakInterval = 3,
            LongBreakMinutes = 20,
            ShortBreakMinutes = 4,
            Cycles = Cycles.Infinite
        });
    }

    [Fact]
    public void MergeGlobalAndLocalSettings_WhenAllLocalSettingsAreProvided_MergedSettingsShouldEqualLocalSettings()
    {
        var globalSettings = new Settings
        {
            FocusMinutes = 10,
            LongBreakInterval = 3,
            LongBreakMinutes = 20,
            ShortBreakMinutes = 4,
            Cycles = 3
        };
        var startArgs = new StartArgs
        {
            FocusMinutes = 20,
            LongBreakInterval = 6,
            LongBreakMinutes = 40,
            ShortBreakMinutes = 8,
            Cycles = 0
        };

        var mergedSettings = this.cut.MergeGlobalAndLocalSettings(globalSettings, startArgs);

        mergedSettings.Should().BeEquivalentTo(new Settings
        {
            FocusMinutes = 20,
            LongBreakInterval = 6,
            LongBreakMinutes = 40,
            ShortBreakMinutes = 8,
            Cycles = 0
        });
    }
}