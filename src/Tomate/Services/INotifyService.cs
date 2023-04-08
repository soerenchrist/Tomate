using Tomate.Models;

namespace Tomate.Services;

public interface INotifyService
{
    void NotifyRemainingFocusTime(Minutes minutes);
    void NotifyRemainingBreakTime(Minutes minutes);
    
}