using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

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
        var dlgVm = _sp.GetRequiredService<AddGameDialogViewModel>();
        var dlg = new AddGameDialog { DataContext = dlgVm, Owner = Application.Current.MainWindow };

        if (dlg.ShowDialog() != true) return;

        var game = new Game
        {
            Title = dlgVm.Title,
            Description = dlgVm.Description,
            Category = dlgVm.SelectedCategory,
            Mode = dlgVm.SelectedMode,
            MaxPlayers = dlgVm.MaxPlayers,
            IsLive = dlgVm.IsLive
        };
        var ver = new GameVersion { Number = dlgVm.Version, FolderPath = dlgVm.ZipPath };

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
        new PlayWindow
        {
            DataContext = new PlayWindowViewModel(g),
            Owner = Application.Current.MainWindow
        }.Show();
    }


}
