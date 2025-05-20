using System.Diagnostics;

namespace GamePlatform.Desktop.Services;

public sealed class NotificationService : INotificationService
{
    public void Show(string message, int ms = 3000)
        => Debug.WriteLine($"[NOTIFY] {message}");
}
