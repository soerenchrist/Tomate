using Tomate.Models;
using Tomate.Services.Abstractions;

namespace Tomate.Services;

public class NotifyService : INotifyService
{
    private readonly INotificationService notificationService;

    public NotifyService(INotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    public async Task NotifyStartOfFocusTime(Minutes minutes)
    {
        await this.notificationService.ShowNotificationAsync("Go back to work!", $"Work for {minutes}...");
        Console.WriteLine("Starting focus time...");
    }

    public Task NotifyRemainingFocusTime(Minutes minutes)
    {
        Console.WriteLine($"Remaining focus time: {minutes}");
        return Task.CompletedTask;
    }

    public async Task NotifyStartOfBreakTime(Minutes minutes)
    {
        await this.notificationService.ShowNotificationAsync("Take a break!", $"Have a break of {minutes}...");
        Console.WriteLine("Take a break...");
    }

    public Task NotifyRemainingBreakTime(Minutes minutes)
    {
        Console.WriteLine($"Remaining break time: {minutes}");
        return Task.CompletedTask;
    }

    public Task NotifyStartOfCycle(Cycles current, Cycles total)
    {
        Console.WriteLine("Starting a new pomodoro cycle...");
        return Task.CompletedTask;
    }
}