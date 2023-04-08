using Tomate.Models;
using Tomate.Services.Abstractions;

namespace Tomate.Services;

public class Scheduler : IScheduler
{
    private readonly IDelay _delay;

    public Scheduler(IDelay delay)
    {
        _delay = delay;
    }
    
    public async IAsyncEnumerable<Minutes> ScheduleMinutes(Minutes minutes, CancellationToken cancellationToken = default)
    {
        var remainingMinutes = minutes.Value;
        while (remainingMinutes > 0)
        {
            if (cancellationToken.IsCancellationRequested) break;
            yield return remainingMinutes;
            await _delay.Delay(TimeSpan.FromMinutes(1));
            remainingMinutes--;
        }
    }
}