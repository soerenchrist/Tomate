using Microsoft.Toolkit.Uwp.Notifications;
using Tomate.Services.Abstractions;

namespace Tomate.Services.Notifications;

public class WindowsNotificationService : INotificationService
{
    public Task ShowNotificationAsync(string title, string message, CancellationToken cancellationToken = default)
    {
#if WINDOWS
        new ToastContentBuilder()
    .AddText(title)
    .AddText(message)
    .Show();
#endif

        return Task.CompletedTask;
    }
}