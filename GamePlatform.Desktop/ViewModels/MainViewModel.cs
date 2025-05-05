using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace GamePlatform.Desktop.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IGameRepository _repo;
        private readonly IServiceProvider _sp;

        public MainViewModel(IGameRepository repo, IServiceProvider sp)
        {
            _repo = repo;
            _sp = sp;
        }

        public ObservableCollection<Game> Games { get; } = new();

        private string _filterText = string.Empty;
        public string FilterText
        {
            get => _filterText;
            set
            {
                if (SetProperty(ref _filterText, value))
                    _ = LoadAsync();
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        [RelayCommand]
        public async Task LoadAsync()
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

            var v = new GameVersion
            {
                Number = dlgVm.Version,
                FolderPath = dlgVm.ZipPath
            };

            await _repo.AddAsync(game, v);
            await LoadAsync();
        }

        [RelayCommand]
        private async Task DeleteAsync(Game? g)
        {
            if (g is null) return;
            await _repo.DeleteAsync(g.Id);
            await LoadAsync();
        }

        [RelayCommand]
        private void Play(Game? g)
        {
            if (g is null) return;
            var win = new PlayWindow
            {
                DataContext = new PlayWindowViewModel(g),
                Owner = App.Current.MainWindow
            };
            win.Show();
        }
    }
}
