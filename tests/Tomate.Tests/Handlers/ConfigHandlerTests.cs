using NSubstitute;
using Tomate.Handlers;
using Tomate.Models;
using Tomate.Models.Cli;
using Tomate.Services.Abstractions;

namespace Tomate.Tests.Handlers;

public class ConfigHandlerTests
{
    private readonly ConfigHandler cut;
    private readonly ISettingsService settingsService = Substitute.For<ISettingsService>();
    private readonly IOutput output = Substitute.For<IOutput>();

    public ConfigHandlerTests()
    {
        cut = new ConfigHandler(settingsService, output);
        settingsService.ReadGlobalSettings()
            .Returns(new Settings
            {
                FocusMinutes = 10,
                Cycles = 5,
                LongBreakInterval = 5,
                LongBreakMinutes = 50,
                ShortBreakMinutes = 8
            });
    }

    [Fact]
    public async Task HandleAsync_WhenArgsAreEmpty_DisplaysMessageToUser()
    {
        await cut.HandleAsync(new ConfigArgs());

        output.Received()
            .WriteErrorLine("No arguments provided. Config was not changed. Use --help to see available options.");
    }

    [Fact]
    public async Task HandleAsync_WhenSingleArgIsPassed_ShouldOnlyChangeSingleValue()
    {
        await cut.HandleAsync(new ConfigArgs
        {
            FocusMinutes = 10
        });

        this.settingsService.Received().UpdateGlobalSettings(new Settings
        {
            FocusMinutes = 10,
            Cycles = 5,
            LongBreakInterval = 5,
            LongBreakMinutes = 50,
            ShortBreakMinutes = 8
        });
    }

    [Fact]
    public async Task HandleAsync_WhenMultipleValuesArePassed_ShouldOnlyChangeThose()
    {
        await cut.HandleAsync(new ConfigArgs
        {
            FocusMinutes = 10,
            Cycles = 10
        });

        this.settingsService.Received().UpdateGlobalSettings(new Settings
        {
            FocusMinutes = 10,
            Cycles = 10,
            LongBreakInterval = 5,
            LongBreakMinutes = 50,
            ShortBreakMinutes = 8
        });
    }

    [Fact]
    public async Task HandleAsync_WhenAllValuesArePassed_ShouldChangeAll()
    {
        await cut.HandleAsync(new ConfigArgs
        {
            FocusMinutes = 10,
            Cycles = 10,
            LongBreakInterval = 8,
            LongBreakMinutes = 10,
            ShortBreakMinutes = 10
        });

        this.settingsService.Received().UpdateGlobalSettings(new Settings
        {
            FocusMinutes = 10,
            Cycles = 10,
            LongBreakInterval = 8,
            LongBreakMinutes = 10,
            ShortBreakMinutes = 10
        });
    }
}