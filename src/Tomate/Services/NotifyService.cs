using Tomate.Models;
using Tomate.Services.Abstractions;

namespace Tomate.Services;

public class NotifyService : INotifyService
{
    public void NotifyStartOfFocusTime()
    {
        Console.WriteLine("Starting focus time...");
    }

    public void NotifyRemainingFocusTime(Minutes minutes)
    {
        Console.WriteLine($"Remaining focus time: {minutes}");
    }

    public void NotifyStartOfBreakTime()
    {
        Console.WriteLine("Take a break...");
    }

    public void NotifyRemainingBreakTime(Minutes minutes)
    {
        Console.WriteLine($"Remaining break time: {minutes}");
    }

    public void NotifyStartOfCycle(Cycles current, Cycles total)
    {
        Console.WriteLine("Starting a new pomodoro cycle...");
    }
}