using Tomate.Models;
using Tomate.Services.Abstractions;

namespace Tomate.Services;

public class NotifyService : INotifyService
{
    private readonly INotificationService notificationService;
    private readonly IOutput output;

    public NotifyService(INotificationService notificationService,
        IOutput output)
    {
        this.notificationService = notificationService;
        this.output = output;
    }

    public async Task NotifyStartOfFocusTime(Minutes minutes)
    {
        await this.notificationService.ShowNotificationAsync("Go back to work!", $"Work for {minutes}...");
        this.output.WriteLine($"Starting focus time. Work for {minutes}...");
    }

    public Task NotifyRemainingFocusTime(Minutes minutes)
    {
        this.output.WriteLine($"Remaining focus time: {minutes}");
        return Task.CompletedTask;
    }

    public async Task NotifyStartOfBreakTime(Minutes minutes)
    {
        await this.notificationService.ShowNotificationAsync("Take a break!", $"Have a break of {minutes}...");
        this.output.WriteLine($"Take a break for {minutes}...");
    }

    public Task NotifyRemainingBreakTime(Minutes minutes)
    {
        this.output.WriteLine($"Remaining break time: {minutes}");
        return Task.CompletedTask;
    }

    public Task NotifyStartOfCycle(Cycles current, Cycles total)
    {
        if (current == Cycles.Infinite || total == Cycles.Infinite)
        {
            this.output.WriteLine("Starting a new pomodoro cycle...");
        }
        else
        {
            this.output.WriteLine($"Starting pomodoro cycle {current.Value} of {total.Value}");
        }
        return Task.CompletedTask;
    }
}