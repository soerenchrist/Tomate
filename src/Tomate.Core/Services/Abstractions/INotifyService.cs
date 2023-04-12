using Tomate.Models;

namespace Tomate.Services.Abstractions;

public interface INotifyService
{
    Task NotifyStartOfFocusTime(Minutes minutes);
    Task NotifyRemainingFocusTime(Minutes minutes);
    Task NotifyStartOfBreakTime(Minutes minutes);
    Task NotifyRemainingBreakTime(Minutes minutes);
    Task NotifyStartOfCycle(Cycles current, Cycles total);
}