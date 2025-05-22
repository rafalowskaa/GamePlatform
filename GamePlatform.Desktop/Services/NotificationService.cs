using System;
using System.Diagnostics;

namespace GamePlatform.Desktop.Services;

public sealed class NotificationService : INotificationService
{
    public event Action<string>? Raised;

    public void Show(string message, int ms = 3000)
    {
        Debug.WriteLine($"[NOTIFY] {message}");
        Raised?.Invoke(message);
    }
}
