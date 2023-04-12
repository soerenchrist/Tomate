using Tmds.DBus;
using Tomate.Services.Abstractions;

namespace Tomate.Services.Notifications;

public class LinuxNotificationService : INotificationService
{
    public async Task ShowNotificationAsync(string title, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = Connection.Session;
            await client.ConnectAsync();

            var proxy = client.CreateProxy<IFreeDesktopNotification>("org.freedesktop.Notifications",
                FreeDesktopNotificationsInfo.Path);
            await ShowNotification(proxy, title, message);
        }
        catch (ConnectException)
        {

        }
    }

    async Task<uint> ShowNotification(IFreeDesktopNotification proxy, string title, string message, uint replaceId = 0)
    {
        var time = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;
        var notificationId = await proxy.NotifyAsync(
            "Tomate",
            replaceId,
            "",
            title,
            message,
            Array.Empty<string>(),
            new Dictionary<string, object>
            {
                { "urgency", (byte)0 }, { "category", "transfer" }
            },
            time
        );
        return notificationId;
    }
}
