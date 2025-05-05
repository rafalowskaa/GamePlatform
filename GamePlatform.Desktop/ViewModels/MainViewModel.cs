using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using GamePlatform.Desktop.Views;
using Microsoft.Extensions.DependencyInjection;

namespace GamePlatform.Desktop.ViewModels;

public partial class MainViewModel(IGameRepository repo, IServiceProvider sp) : ObservableObject
{
    public ObservableCollection<Game> Games { get; } = new();
    [ObservableProperty] private string filterText = string.Empty;
    [ObservableProperty] private bool isBusy;

    [RelayCommand] public async Task LoadAsync() { IsBusy = true; var list = await repo.GetAllAsync(FilterText); Games.Clear(); foreach (var g in list) Games.Add(g); IsBusy = false; }

    [RelayCommand]
    private async Task AddGameAsync()
    {
        var dlgVm = sp.GetRequiredService<AddGameDialogViewModel>();
        var dlg = new AddGameDialog { DataContext = dlgVm, Owner = App.Current.MainWindow };
        if (dlg.ShowDialog() != true) return;

        var game = new Game
        {
            Title = dlgVm.Title,
            Description = dlgVm.Description,
            Category = dlgVm.SelectedCategory,
            Mode = dlgVm.SelectedMode,
            MaxPlayers = dlgVm.MaxPlayers,
            IsLive = dlgVm.IsLive,
            IsApproved = false
        };
        var v = new GameVersion { Number = dlgVm.Version, FolderPath = dlgVm.ZipPath };
        await repo.AddAsync(game, v);
        await LoadAsync();
    }

    [RelayCommand]
    private async Task DeleteAsync(Game? g)
    {
        if (g is null) return;
        await repo.DeleteAsync(g.Id);
        await LoadAsync();
    }

    partial void OnFilterTextChanged(string value) => _ = LoadAsync();
}