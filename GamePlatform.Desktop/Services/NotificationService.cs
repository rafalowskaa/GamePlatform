using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;

namespace GamePlatform.Desktop.Services;

public sealed class NotificationService : INotificationService
{
    private readonly string _file;
    public event Action<string>? Raised;
    public ObservableCollection<string> History { get; } = new();

    public NotificationService()
    {
        _file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                             "GamePlatform", "notifications.log");
        Directory.CreateDirectory(Path.GetDirectoryName(_file)!);
        if (File.Exists(_file))
        {
            foreach (var line in File.ReadAllLines(_file))
                History.Add(line);

        }
    }

    public void Show(string message, int ms = 3000)
    {
        Debug.WriteLine($"[NOTIFY] {message}");
        History.Add(message);
        try
        {
            File.AppendAllLines(_file, [message]);
        }
        catch { }
        Raised?.Invoke(message);
    }

    public Task<bool> ConfirmAsync(string message, int _)  // ms ignorujemy – MessageBox blokuje
    {
        var res = MessageBox.Show(message, "Potwierdź", MessageBoxButton.YesNo,
                                   MessageBoxImage.Question);
        return Task.FromResult(res == MessageBoxResult.Yes);
    }
}