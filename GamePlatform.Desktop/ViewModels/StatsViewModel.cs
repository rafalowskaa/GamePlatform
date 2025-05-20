using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>VM zakładki „Statystyki / Leaderboard”.</summary>
public partial class StatsViewModel : ObservableObject
{
    private readonly IGameRepository _repo;
    public StatsViewModel(IGameRepository repo) => _repo = repo;

    public ObservableCollection<Game> Games { get; } = new();
    public ObservableCollection<Score> Top { get; } = new();

    /* -------------- wybrana gra -------------- */
    [ObservableProperty] private Game? _selectedGame;

    /// <summary>
    /// Inicjalizacja listy gier przekazanej z <see cref="MainViewModel"/>.
    /// </summary>
    public void Init(System.Collections.Generic.IEnumerable<Game> allGames)
    {
        Games.Clear();
        foreach (var g in allGames)
            Games.Add(g);

        SelectedGame = Games.FirstOrDefault();   // ✱ tu był błąd
    }

    /* wywoływane automatycznie przez source-generator */
    partial void OnSelectedGameChanged(Game? oldValue, Game? newValue)
    {
        _ = RefreshAsync();        //  „fire-and-forget”
    }

    /* -------------- COMMAND -------------- */
    [RelayCommand]
    private async Task RefreshAsync()
    {
        Top.Clear();
        if (SelectedGame is null) return;

        var list = await _repo.GetTopScoresAsync(SelectedGame.Id, 50);
        foreach (var s in list) Top.Add(s);
    }
}
