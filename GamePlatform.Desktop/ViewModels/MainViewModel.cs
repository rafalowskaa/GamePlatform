using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GamePlatform.Shared.Enums;
using GamePlatform.Desktop.Views;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>Lista gier – widok wspólny dla użytkownika i admina.</summary>
public partial class MainViewModel : ObservableObject
{
    private readonly IGameRepository _repo;
    private readonly IServiceProvider _sp;

    [ObservableProperty] private Game? _selectedGame;

    public MainViewModel(IGameRepository repo, IServiceProvider sp)
    {
        _repo = repo;
        _sp = sp;
        _ = RefreshAsync();
    }

    /* ---------------- observable props ---------------- */
    public ObservableCollection<Game> Games { get; } = new();

    [ObservableProperty] private string _filterText = "";
    partial void OnFilterTextChanged(string? _unused)   // param nieużywany
        => _ = RefreshAsync();

    [ObservableProperty] private bool _isBusy;

    /* ---------------- commands (Toolkit) -------------- */

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsBusy = true;
        var list = await _repo.GetAllAsync(FilterText);
        Games.Clear();
        foreach (var g in list) Games.Add(g);
        IsBusy = false;
    }

    [RelayCommand]
    private async Task AddGameAsync()
    {
        var dlg = new Microsoft.Win32.OpenFileDialog { Filter = "Executable|*.exe" };

        if (dlg.ShowDialog() != true) return;

        var game = new Game
        {
            Title = System.IO.Path.GetFileNameWithoutExtension(dlg.FileName),
            Category = GameCategory.Other,
            Mode = GameMode.SinglePlayer,
            IsLive = false
        };
        var ver = new GameVersion { Number = "1.0", FolderPath = dlg.FileName };

        await _repo.AddAsync(game, ver);
        Games.Add(game);
    }

    [RelayCommand]
    private async Task DeleteAsync(Game? g)
    {
        if (g is null) return;
        await _repo.DeleteAsync(g.Id);
        await RefreshAsync();
    }

    [RelayCommand]
    private void Play(Game? g)   // zostawiamy param
    {
        g ??= SelectedGame;
        if (g is null) return;
        var latest = g.Versions.OrderByDescending(v => v.Number).FirstOrDefault();
        if (latest is null) return;

        string html = Path.Combine(latest.FolderPath, "index.html");
        if (File.Exists(html))
        {
            new PlayWindow
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
                try { Process.Start(new ProcessStartInfo(exe) { UseShellExecute = true }); }
                catch { }
            }
        }
    }


}
