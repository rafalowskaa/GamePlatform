using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>Lista gier – widok wspólny dla użytkownika i admina.</summary>
public partial class MainViewModel : ObservableObject
{
    private readonly IGameRepository _repo;
    private readonly IServiceProvider _sp;

    public bool IsAdmin =>
        _sp.GetRequiredService<IAuthService>().CurrentRole is UserRole.Admin or UserRole.Moderator;


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
    partial void OnFilterTextChanged(string? oldValue, string? newValue)
    {
        _ = RefreshAsync();
    }

    [ObservableProperty] private GameMode? _selectedMode;
    partial void OnSelectedModeChanged(GameMode? oldValue, GameMode? newValue)
    {
        _ = RefreshAsync();
    }

    [ObservableProperty] private GameCategory? _selectedCategory;
    partial void OnSelectedCategoryChanged(GameCategory? oldValue, GameCategory? newValue)
    {
        _ = RefreshAsync();
    }

    public IEnumerable<GameMode> ModeValues => EnumExtensions.ModeValues;
    public IEnumerable<GameCategory> CategoryValues => EnumExtensions.CategoryValues;

    public IEnumerable<string> SortOptions { get; } = new[] { "Tytuł", "Data" };
    [ObservableProperty] private string? _selectedSort;
    partial void OnSelectedSortChanged(string? oldValue, string? newValue)
    {
        _ = RefreshAsync();
    }

    [ObservableProperty] private bool _isBusy;

    /* ---------------- commands (Toolkit) -------------- */

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsBusy = true;
        var list = await _repo.GetAllAsync(FilterText);
        var favs = await _repo.GetFavoritesAsync(_sp.GetRequiredService<IAuthService>().CurrentUser!.Id);
        var favIds = favs.Select(f => f.Id).ToHashSet();
        foreach (var g in list) g.IsFavorite = favIds.Contains(g.Id);

        IEnumerable<Game> query = list;
        if (SelectedMode is GameMode m)
            query = query.Where(g => g.Mode == m);
        if (SelectedCategory is GameCategory c)
            query = query.Where(g => g.Category == c);
        query = SelectedSort switch
        {
            "Tytuł" => query.OrderBy(g => g.Title),
            "Data" => query.OrderByDescending(g => g.SubmittedAt),
            _ => query
        };
        Games = new ObservableCollection<Game>(query);
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
    private async Task ToggleFavoriteAsync(Game? g)
    {
        g ??= SelectedGame;
        if (g is null) return;
        var userId = _sp.GetRequiredService<IAuthService>().CurrentUser!.Id;
        await _repo.ToggleFavoriteAsync(userId, g.Id);
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
