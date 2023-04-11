using Tmds.DBus;

namespace Tomate.Services.Abstractions;

[DBusInterface("org.freedesktop.Notifications")]
public interface IFreeDesktopNotification : IDBusObject
{
    Task<uint> NotifyAsync(string appName, uint replacesId, string? appIcon, string? summary, string? body,
        string[] actions, IDictionary<string, object> hints, int expireTimeout);

    Task CloseNotificationAsync(uint id);

    Task<string[]> GetCapabilitiesAsync();

    Task<(string name, string vendor, string version, string spec_version)> GetServerInformationAsync();

    Task<IDisposable> WatchNotificationClosedAsync(Action<(uint id, uint reason)> handler,
        Action<Exception>? onError = null);

    Task<IDisposable> WatchActionInvokedAsync(Action<(uint id, string action_key)> handler,
        Action<Exception>? onError = null);
}

public static class FreeDesktopNotificationsInfo
{
    public static readonly ObjectPath Path = new ObjectPath("/org/freedesktop/Notifications");
}