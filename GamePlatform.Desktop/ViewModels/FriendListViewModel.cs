using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>Lista znajomych + wyszukiwarka i zapraszanie graczy.</summary>
public partial class FriendListViewModel : ObservableObject
{
    private readonly IFriendService _friends;
    private readonly IAuthService _auth;
    private readonly INotificationService _notify;

    public FriendListViewModel(IFriendService friends,
                               IAuthService auth,
                               INotificationService notify)
    {
        _friends = friends;
        _auth = auth;
        _notify = notify;

        _ = RefreshAsync();
    }

    /* ---------- kolekcje ---------- */

    public ObservableCollection<User> Friends { get; } = new();
    public ObservableCollection<User> Suggestions { get; } = new();

    /* ---------- observable props ---------- */

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string _searchText = "";
    [ObservableProperty] private User? _selectedFriend;
    [ObservableProperty] private User? _selectedSuggestion;
    [ObservableProperty] private bool _isSuggestOpen;

    // ← hook generowany przez Toolkit
    partial void OnSearchTextChanged(string value)
        => _ = FindSuggestionsAsync();     // fire-and-forget ok

    /* ---------- commands ---------- */

    [RelayCommand]                       // odśwież listę
    private async Task RefreshAsync()
    {
        if (_auth.CurrentUser is null) return;

        IsBusy = true;
        Friends.Clear();
        foreach (var u in await _friends.GetFriendsAsync(_auth.CurrentUser.Id))
            Friends.Add(u);
        IsBusy = false;
    }

    [RelayCommand]                       // usuń znajomego
    private async Task RemoveAsync(User? u)
    {
        if (u is null || _auth.CurrentUser is null) return;

        await _friends.RemoveFriendAsync(_auth.CurrentUser.Id, u.Id);
        _notify.Info($"Usunięto {u.Login} z listy znajomych");
        await RefreshAsync();
    }

    [RelayCommand]                       // wyślij zaproszenie
    private async Task InviteAsync()
    {
        if (_auth.CurrentUser is null || SelectedSuggestion is null) return;

        await _friends.SendRequestAsync(
            _auth.CurrentUser.Id, SelectedSuggestion.Id);

        _notify.Info($"Wysłano zaproszenie do {SelectedSuggestion.Login}");
        SearchText = "";
        IsSuggestOpen = false;
    }

    // ===== komenda wywoływana z FriendsPage (Enter w pola) =====
    [RelayCommand]
    private async Task FindSuggestions()
        => await FindSuggestionsAsync();

    /* ---------- autosugestia ---------- */

    private async Task FindSuggestionsAsync()
    {
        Suggestions.Clear();

        if (string.IsNullOrWhiteSpace(SearchText) || _auth.CurrentUser is null)
        {
            IsSuggestOpen = false;
            return;
        }

        var list = await _friends.FindUsersAsync(SearchText, _auth.CurrentUser.Id);
        foreach (var u in list) Suggestions.Add(u);

        IsSuggestOpen = Suggestions.Count > 0;
    }
}
