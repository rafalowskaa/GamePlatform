using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

public partial class FriendListViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly IFriendService _friends;
    private readonly IChatService _chat;
    private readonly INotificationService _notify;
    private readonly IServiceProvider _sp;

    public FriendListViewModel(IServiceProvider sp,
                               IAuthService auth,
                               IFriendService friends,
                               IChatService chat,
                               INotificationService notify)
    {
        _sp = sp;
        _auth = auth;
        _friends = friends;
        _chat = chat;
        _notify = notify;

        _ = RefreshFriendsAsync();   // fire-and-forget przy starcie
    }

    /* =====  MODEL  ===== */
    public ObservableCollection<User> Friends { get; } = new();
    public ObservableCollection<User> Online { get; } = new();

    [ObservableProperty] private string _searchText = "";
    [ObservableProperty] private bool _isSuggestOpen = false;
    [ObservableProperty] private User? _selectedFriend;

    /* ========== REAKCJA NA WPISYWANY TEKST ========== */
    partial void OnSearchTextChanged(string? _) => _ = RefreshSuggestionsAsync();

    /* ==========  KOMENDY  ========== */

    [RelayCommand] private Task RefreshFriendsAsync() => LoadFriendsAsync();
    [RelayCommand] private Task RefreshSuggestionsAsync() => LoadSuggestionsAsync();
    [RelayCommand]
    private void StartChat(User? mate)
    {
        if (mate is null) return;
        ChatWindow.Open(_auth.CurrentUser!, mate);      // singleton-like otwarcie okna
    }

    [RelayCommand]
    private async Task ToggleFriendAsync()
    {
        if (SelectedFriend is null) return;

        var me = _auth.CurrentUser!.Id;
        var mate = SelectedFriend.Id;

        await _friends.ToggleFriendAsync(me, mate);
        await RefreshFriendsAsync();

        _notify.Info($"Zmieniono relację z użytkownikiem {SelectedFriend.Login}");
    }

    /* ==========  PRYWATNE  ========== */

    private async Task LoadFriendsAsync()
    {
        Friends.Clear();
        Online.Clear();

        if (_auth.CurrentUser is null) return;

        foreach (var u in await _friends.GetFriendsAsync(_auth.CurrentUser.Id))
        {
            Friends.Add(u);
            if (await _friends.IsUserOnlineAsync(u.Id))
                Online.Add(u);
        }
    }

    private async Task LoadSuggestionsAsync()
    {
        // prostackie wyszukiwanie po prefiksie
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            IsSuggestOpen = false;
            return;
        }

        var list = await _sp.GetRequiredService<IProfileService>()
                            .FindByLoginAsync(SearchText!, take: 8);

        Suggestions.Clear();
        foreach (var u in list) Suggestions.Add(u);

        IsSuggestOpen = Suggestions.Count > 0;
    }

    public ObservableCollection<User> Suggestions { get; } = new();
}
