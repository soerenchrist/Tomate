namespace Tomate.Services.Abstractions;

public interface INotificationService
{
    Task ShowNotificationAsync(string title, string message, CancellationToken cancellationToken = default);
}