using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>
/// Zakładka „Znajomi” – lista, wyszukiwanie, zapraszanie, blokowanie itp.
/// </summary>
public partial class FriendListViewModel : ObservableObject
{
    private readonly IFriendService _friends;
    private readonly IAuthService _auth;
    private readonly INotificationService _notify;

    public FriendListViewModel(
        IFriendService friends,
        IAuthService auth,
        INotificationService notify)
    {
        _friends = friends;
        _auth = auth;
        _notify = notify;

        _ = RefreshAsync();                // startowe wczytanie listy
    }

    /* ─────  KOLEKCJE  ───── */
    public ObservableCollection<User> Friends { get; } = new();
    public ObservableCollection<User> Suggestions { get; } = new();

    /* ─────  STAN  ───── */
    [ObservableProperty] private string _searchText = "";
    [ObservableProperty] private bool _isSuggestOpen;
    [ObservableProperty] private User? _selectedSuggestion;

    /* wpis w polu → podpowiedzi */
    partial void OnSearchTextChanged(string _oldValue)
    {
        _ = PerformSuggestAsync();         // fire-and-forget
    }

    /* ─────  KOMENDY (Toolkit)  ───── */
    [RelayCommand] private Task RefreshAsync() => LoadFriendsAsync();
    [RelayCommand] private Task InviteAsync() => SendInviteAsync();
    [RelayCommand] private Task FindSuggestionsAsync() => PerformSuggestAsync();

    [RelayCommand] private Task ViewProfileAsync(User? u) => ShowProfileAsync(u);
    [RelayCommand] private Task BlockAsync(User? u) => BlockUserAsync(u);
    [RelayCommand] private Task UnblockAsync(User? u) => UnblockUserAsync(u);
    [RelayCommand] private Task RemoveAsync(User? u) => RemoveFriendAsync(u);

    /* ─────  LOGIKA  ───── */

    private async Task LoadFriendsAsync()
    {
        if (_auth.CurrentUser is null) return;

        Friends.Clear();
        foreach (var u in await _friends.GetFriendsAsync(_auth.CurrentUser.Id))
            Friends.Add(u);
    }

    private async Task SendInviteAsync()
    {
        if (_auth.CurrentUser is null || SelectedSuggestion is null) return;

        await _friends.SendRequestAsync(_auth.CurrentUser.Id, SelectedSuggestion.Id);
        _notify.Show($"‣ Wysłano zaproszenie do {SelectedSuggestion.Login}");
        SearchText = "";
        IsSuggestOpen = false;
    }

    private async Task PerformSuggestAsync()
    {
        Suggestions.Clear();

        if (string.IsNullOrWhiteSpace(SearchText) || _auth.CurrentUser is null)
        { IsSuggestOpen = false; return; }

        foreach (var u in await _friends.FindUsersAsync(SearchText, _auth.CurrentUser.Id))
            Suggestions.Add(u);

        IsSuggestOpen = Suggestions.Count > 0;
    }

    /* ---- akcje z menu kontekstowego ---- */

    private Task ShowProfileAsync(User? u)
    {
        if (u is null) return Task.CompletedTask;
        _notify.Show($"Profil użytkownika „{u.Login}” – TODO");
        return Task.CompletedTask;
    }

    private async Task BlockUserAsync(User? u)
    {
        if (u is null || _auth.CurrentUser is null) return;
        await _friends.BlockAsync(_auth.CurrentUser.Id, u.Id);
        _notify.Show($"Zablokowano {u.Login}");
    }

    private async Task UnblockUserAsync(User? u)
    {
        if (u is null || _auth.CurrentUser is null) return;
        await _friends.UnblockAsync(_auth.CurrentUser.Id, u.Id);
        _notify.Show($"Odblokowano {u.Login}");
    }

    private async Task RemoveFriendAsync(User? u)
    {
        if (u is null || _auth.CurrentUser is null) return;
        await _friends.RemoveFriendAsync(_auth.CurrentUser.Id, u.Id);
        _notify.Show($"Usunięto {u.Login} z listy znajomych");
        await LoadFriendsAsync();
    }
}
