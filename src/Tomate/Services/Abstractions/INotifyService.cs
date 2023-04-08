using Tomate.Models;

namespace Tomate.Services.Abstractions;

public interface INotifyService
{
    void NotifyStartOfFocusTime();
    void NotifyRemainingFocusTime(Minutes minutes);
    void NotifyStartOfBreakTime();
    void NotifyRemainingBreakTime(Minutes minutes);
    void NotifyStartOfCycle(Cycles current, Cycles total);
    
}