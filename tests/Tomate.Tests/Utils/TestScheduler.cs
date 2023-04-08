using Tomate.Models;
using Tomate.Services.Abstractions;

namespace Tomate.Tests.Utils;

public class TestScheduler : IScheduler
{
    public async IAsyncEnumerable<Minutes> ScheduleMinutes(Minutes minutes, CancellationToken cancellationToken = default)
    {
        var remainingMinutes = minutes;
        while (remainingMinutes > 0)
        {
            if (cancellationToken.IsCancellationRequested) break;
            yield return remainingMinutes;
            await Task.Delay(0, cancellationToken);
            remainingMinutes--;
        }
    }
}