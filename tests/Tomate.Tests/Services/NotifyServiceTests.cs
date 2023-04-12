using Tomate.Models;
using Tomate.Services;
using Tomate.Services.Abstractions;

namespace Tomate.Tests.Services;

public class NotifyServiceTests
{
    private readonly INotificationService notificationService;
    private readonly IOutput output;
    private readonly NotifyService cut;

    public NotifyServiceTests()
    {
        notificationService = Substitute.For<INotificationService>();
        output = Substitute.For<IOutput>();
        cut = new NotifyService(notificationService, output);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(25)]
    [InlineData(30)]
    public async Task NotifyStartOfFocusTime_ShouldSendNotificationAndOutputMessage_ContainingMinutes(int minutes)
    {
        await cut.NotifyStartOfFocusTime(minutes);
        await this.notificationService.Received().ShowNotificationAsync("Go back to work!", $"Work for {minutes}min...");
        this.output.Received().WriteLine($"Starting focus time. Work for {minutes}min...");
    }

    [Theory]
    [InlineData(5)]
    [InlineData(25)]
    [InlineData(30)]
    public async Task NotifyRemainingFocusTime_ShouldOutputMessage(int minutes)
    {
        await cut.NotifyRemainingFocusTime(minutes);
        this.output.Received().WriteLine($"Remaining focus time: {minutes}min");
        await this.notificationService.DidNotReceive().ShowNotificationAsync(Arg.Any<string>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData(5)]
    [InlineData(25)]
    [InlineData(30)]
    public async Task NotifyStartOfBreakTime_ShouldSendNotificationAndOutputMessage_ContainingMinutes(int minutes)
    {
        await cut.NotifyStartOfBreakTime(minutes);
        await this.notificationService.Received().ShowNotificationAsync("Take a break!", $"Have a break of {minutes}min...");
        this.output.Received().WriteLine($"Take a break for {minutes}min...");
    }

    [Theory]
    [InlineData(5)]
    [InlineData(25)]
    [InlineData(30)]
    public async Task NotifyRemainingBreakTime_ShouldOutputMessage(int minutes)
    {
        await cut.NotifyRemainingBreakTime(minutes);
        this.output.Received().WriteLine($"Remaining break time: {minutes}min");
        await this.notificationService.DidNotReceive().ShowNotificationAsync(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task NotifyStartOfCycle_ShouldOnlyOutputGenericMessage_WhenCycleIsInfinite()
    {
        await cut.NotifyStartOfCycle(Cycles.Infinite, Cycles.Infinite);
        this.output.Received().WriteLine($"Starting a new pomodoro cycle...");
        await this.notificationService.DidNotReceive().ShowNotificationAsync(Arg.Any<string>(), Arg.Any<string>());
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 3)]
    [InlineData(3, 3)]
    public async Task NotifyStartOfCycle_ShouldOutputMessageWithCycleNumbers_WhenCycleIsDefinite(int current, int total)
    {
        await cut.NotifyStartOfCycle(current, total);
        this.output.Received().WriteLine($"Starting pomodoro cycle {current} of {total}");
        await this.notificationService.DidNotReceive().ShowNotificationAsync(Arg.Any<string>(), Arg.Any<string>());
    }
}