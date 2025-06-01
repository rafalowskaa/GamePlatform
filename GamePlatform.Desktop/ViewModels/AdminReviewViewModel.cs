using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GamePlatform.Desktop.ViewModels;

public partial class AdminReviewViewModel : ObservableObject
{
    private readonly IGameRepository _repo;
    private readonly INotificationService _notify;
    private readonly IServiceProvider _sp;

    public AdminReviewViewModel(IGameRepository repo, INotificationService notify, IServiceProvider sp)
    {
        _repo = repo;
        _notify = notify;
        _sp = sp;
        _ = RefreshAsync();
    }

    [ObservableProperty]
    private ObservableCollection<Game> _games = new();

    [RelayCommand]
    private async Task RefreshAsync()
    {
        Games = new ObservableCollection<Game>(await _repo.GetPendingAsync());
    }

    [RelayCommand]
    private void Play(Game? g)
    {
        if (g is null) return;
        var latest = g.Versions.OrderByDescending(v => v.Number).FirstOrDefault();
        if (latest is null) return;
        string html = Path.Combine(latest.FolderPath, "index.html");
        if (File.Exists(html))
        {
            new GamePlatform.Desktop.Views.PlayWindow
            {
                DataContext = new PlayWindowViewModel(g),
                Owner = Application.Current.MainWindow
            }.Show();
        }
        else
        {
            string? exe = Directory.GetFiles(latest.FolderPath, "*.exe").FirstOrDefault();
            if (exe is not null)
            {
                try { Process.Start(new ProcessStartInfo(exe) { UseShellExecute = true }); } catch { }
            }
        }
    }

    [RelayCommand]
    private async Task ApproveAsync(Game? g)
    {
        if (g is null) return;
        g.IsApproved = true;
        await _repo.UpdateAsync(g);
        await RefreshAsync();
    }

    [RelayCommand]
    private void RequestFix(Game? g)
    {
        if (g is null) return;
        _notify.PushToUser(g.SubmittedBy, $"Administrator prosi o poprawki gry {g.Title}");
    }

    [RelayCommand]
    private async Task DeleteAsync(Game? g)
    {
        if (g is null) return;
        await _repo.DeleteAsync(g.Id);
        await RefreshAsync();
    }
}