using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
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
    [ObservableProperty]
    private ObservableCollection<Game> _games = new();

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
        Games = new ObservableCollection<Game>(list);
        IsBusy = false;
    }

    [RelayCommand]
    private async Task AddGameAsync()
    {
        var vm = _sp.GetRequiredService<AddGameDialogViewModel>();
        var win = new AddGameDialog { DataContext = vm, Owner = Application.Current.MainWindow };
        if (win.ShowDialog() != true) return;

        var me = _sp.GetRequiredService<IAuthService>().CurrentUser!;

        var game = new Game
        {
            Title = vm.Title,
            Description = vm.Description,
            Category = vm.SelectedCategory,
            Mode = vm.SelectedMode,
            MaxPlayers = vm.MaxPlayers,
            IsLive = vm.IsLive,
            CoverPath = vm.CoverPath,
            SubmittedBy = me.Id,
            SubmittedAt = DateTime.UtcNow
        };
        var ver = new GameVersion { Number = vm.Version, FolderPath = vm.ZipPath };

        await _repo.AddAsync(game, ver);
        await RefreshAsync();
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

        string? html = Directory.GetFiles(latest.FolderPath, "index.html", SearchOption.AllDirectories).FirstOrDefault();
        if (html is not null)
        {
            new PlayWindow
            {
                DataContext = new PlayWindowViewModel(g, html),
                Owner = Application.Current.MainWindow
            }.Show();
        }
        else
        {
            string? exe = Directory.GetFiles(latest.FolderPath, "*.exe", SearchOption.AllDirectories).FirstOrDefault();
            if (exe is not null)
            {
                try { Process.Start(new ProcessStartInfo(exe) { UseShellExecute = true }); }
                catch { }
            }
        }
    }


}
