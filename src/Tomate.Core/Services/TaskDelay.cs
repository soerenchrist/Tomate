using Tomate.Services.Abstractions;

namespace Tomate.Services;

public class TaskDelay : IDelay
{
    public Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken = default)
    {
        return Task.Delay(timeSpan, cancellationToken);
    }
}