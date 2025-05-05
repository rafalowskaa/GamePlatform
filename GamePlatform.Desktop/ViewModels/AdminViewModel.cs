using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels
{
    public partial class AdminViewModel
    {
        public ObservableCollection<Game> Games { get; } = new();
        private readonly IGameRepository _repo;

        public AdminViewModel(IServiceProvider sp)
        {
            _repo = sp.GetRequiredService<IGameRepository>();
            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var list = await _repo.GetAllAsync();
            Games.Clear();
            foreach (var g in list) Games.Add(g);
        }
    }
}
