using Tomate.Models;

namespace Tomate.Services.Abstractions;

public interface IScheduler
{
    IAsyncEnumerable<Minutes> ScheduleMinutes(Minutes minutes, CancellationToken cancellationToken = default);
}