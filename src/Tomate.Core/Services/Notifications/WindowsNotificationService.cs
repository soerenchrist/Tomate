using Microsoft.Toolkit.Uwp.Notifications;
using Tomate.Services.Abstractions;

namespace Tomate.Services.Notifications;

public class WindowsNotificationService : INotificationService
{
    public Task ShowNotificationAsync(string title, string message, CancellationToken cancellationToken = default)
    {
#if WINDOWS
        new ToastContentBuilder()
    .AddArgument("action", "viewConversation")
    .AddArgument("conversationId", 9813)
    .AddText("Andrew sent you a picture")
    .AddText("Check this out, The Enchantments in Washington!")
    .Show();
#endif

        return Task.CompletedTask;
    }
}