using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace GamePlatform.Desktop.Services;

public sealed class NotificationService : INotificationService
{
    public event Action<string>? Raised;

    public void Show(string message, int ms = 3000)
    {
        Debug.WriteLine($"[NOTIFY] {message}");
        Raised?.Invoke(message);
    }

    public Task<bool> ConfirmAsync(string message, int _)  // ms ignorujemy – MessageBox blokuje
    {
        var res = MessageBox.Show(message, "Potwierdź", MessageBoxButton.YesNo,
                                   MessageBoxImage.Question);
        return Task.FromResult(res == MessageBoxResult.Yes);
    }
}