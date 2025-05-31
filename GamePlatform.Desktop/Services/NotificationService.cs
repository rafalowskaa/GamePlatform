using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.IO;

namespace GamePlatform.Desktop.Services;

public sealed class NotificationService : INotificationService
{
    private string _file = null!;
    public event Action<string>? Raised;
    public ObservableCollection<string> History { get; } = new();

    public NotificationService() { }

    public void LoadForUser(Guid userId)
    {
        _file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                             "GamePlatform", $"notifications_{userId}.log");
        Directory.CreateDirectory(Path.GetDirectoryName(_file)!);
        History.Clear();
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
        if (!string.IsNullOrEmpty(_file))
        {
            try { File.AppendAllLines(_file, [message]); } catch { }
        }
        
        Raised?.Invoke(message);
    }

    public void Remove(string message)
    {
        if (History.Remove(message) && !string.IsNullOrEmpty(_file))
        {
            try { File.WriteAllLines(_file, History); } catch { }
        }
    }

    public Task<bool> ConfirmAsync(string message, int _)  // ms ignorujemy – MessageBox blokuje
    {
        var res = MessageBox.Show(message, "Potwierdź", MessageBoxButton.YesNo,
                                   MessageBoxImage.Question);
        return Task.FromResult(res == MessageBoxResult.Yes);
    }
}