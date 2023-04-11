using NSubstitute;
using Tomate.Handlers;
using Tomate.Models;
using Tomate.Models.Cli;
using Tomate.Services.Abstractions;

namespace Tomate.Tests.Handlers;

public class ListConfigHandlerTests
{
    private readonly ListConfigHandler cut;
    private readonly IOutput output = Substitute.For<IOutput>();
    private readonly ISettingsService settingsService = Substitute.For<ISettingsService>();

    public ListConfigHandlerTests()
    {
        this.cut = new ListConfigHandler(this.settingsService, this.output);
    }
    
    [Fact]
    public async Task HandleAsync_WhenCalled_ShouldDisplayCurrentSettings()
    {
        this.settingsService.ReadGlobalSettings().Returns(new Settings
        {
            FocusMinutes = 10,
            Cycles = 5,
            LongBreakInterval = 5,
            LongBreakMinutes = 50,
            ShortBreakMinutes = 8
        });
        
        await this.cut.HandleAsync(new ListConfigArgs());
        
        this.output.Received().WriteLine("Focus time: 10min");
        this.output.Received().WriteLine("Short break: 8min");
        this.output.Received().WriteLine("Long break: 50min");
        this.output.Received().WriteLine("Long break interval: 5 times");
        this.output.Received().WriteLine("Cycles: 5 times");
    }
    
    [Fact]
    public async Task HandleAsync_ShouldDisplayInfinite_WhenCyclesIsZero()
    {
        this.settingsService.ReadGlobalSettings().Returns(new Settings
        {
            FocusMinutes = 10,
            Cycles = 0,
            LongBreakInterval = 5,
            LongBreakMinutes = 50,
            ShortBreakMinutes = 8
        });
        
        await this.cut.HandleAsync(new ListConfigArgs());
        
        this.output.Received().WriteLine("Cycles: Infinite");
    }
}