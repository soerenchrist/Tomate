namespace Tomate.Services.Abstractions;

public interface IDelay
{
    Task Delay(TimeSpan timeSpan, CancellationToken cancellationToken = default);
}