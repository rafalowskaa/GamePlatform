using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.Identity;

namespace GamePlatform.Desktop.ViewModels;

public partial class FriendListViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly IFriendService _friends;
    private readonly IChatService _chat;
    private readonly INotificationService _notify;
    private readonly IServiceProvider _sp;
    private readonly HashSet<Guid> _friendIds = new();

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

        _chat.PresenceChanged += OnPresenceChanged;

        _ = RefreshFriendsAsync();   // fire-and-forget przy starcie
    }

    ~FriendListViewModel()
    {
        _chat.PresenceChanged -= OnPresenceChanged;
    }

    /* =====  MODEL  ===== */
    public ObservableCollection<User> Friends { get; } = new();
    public ObservableCollection<User> Online { get; } = new();
    public ObservableCollection<User> Blocked { get; } = new();

    [ObservableProperty] private string _searchText = "";
    [ObservableProperty] private bool _isSuggestOpen = false;
    [ObservableProperty] private User? _selectedFriend;
    [ObservableProperty] private bool _friendsExpanded = true;
    [ObservableProperty] private bool _blockedExpanded = false;

    /* ========== REAKCJA NA WPISYWANY TEKST ========== */
    partial void OnSearchTextChanged(string? newValue)
    {
        _ = RefreshSuggestionsAsync();
    }

    /* ==========  KOMENDY  ========== */

    [RelayCommand] private Task RefreshFriendsAsync() => LoadFriendsAsync();
    [RelayCommand] private Task RefreshSuggestionsAsync() => LoadSuggestionsAsync();

    [RelayCommand]
    private void AddFriend()
    {
        var page = new FindFriendsPage { DataContext = _sp.GetRequiredService<FindFriendsViewModel>() };
        var win = new System.Windows.Window
        {
            Content = page,
            Owner = System.Windows.Application.Current.MainWindow,
            Width = 400,
            Height = 500,
            Title = "Dodaj znajomego"
        };
        win.ShowDialog();
        _ = RefreshFriendsAsync();
    }

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

    [RelayCommand]
    private async Task ToggleForAsync(User? user)
    {
        if (user is null) return;

        await _friends.ToggleFriendAsync(_auth.CurrentUser!.Id, user.Id);
        await RefreshFriendsAsync();
    }


    [RelayCommand]
    private async Task BlockAsync(User? target)
    {
        if (target is null) return;

        if (!await _notify.ConfirmAsync($"Czy na pewno zablokować użytkownika {target.Login}?", 0))
            return;

        var pwdDlg = new PasswordPromptWindow { Owner = Application.Current.MainWindow };
        if (pwdDlg.ShowDialog() != true) return;

        var me = _auth.CurrentUser;
        if (me is null) return;

        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
        if (hasher.VerifyHashedPassword(me, me.PasswordHash, pwdDlg.Password)
            == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
        {
            MessageBox.Show("Nieprawidłowe hasło.", "Błąd",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        await _friends.BlockAsync(me.Id, target.Id);

        await RefreshFriendsAsync();
    }

    [RelayCommand]
    private void ShowProfile(User? target)
    {
        if (target is null) return;
        var dlg = new UserProfileDialog
        {
            DataContext = target,
            Owner = Application.Current.MainWindow
        };
        dlg.ShowDialog();
    }

    [RelayCommand]
    private void Report(User? target)
    {
        if (target is null) return;
        var vm = new ReportDialogViewModel(_sp.GetRequiredService<IReportService>(), _auth.CurrentUser!.Id, target.Id);
        var dlg = new ReportDialog { DataContext = vm, Owner = System.Windows.Application.Current.MainWindow };
        dlg.ShowDialog();
    }

    /* ==========  PRYWATNE  ========== */

    private async Task LoadFriendsAsync()
    {
        Friends.Clear();
        Online.Clear();
        Blocked.Clear();
        _friendIds.Clear();

        if (_auth.CurrentUser is null) return;

        var friends = await _friends.GetFriendsAsync(_auth.CurrentUser.Id);
        foreach (var u in friends)
        {
            u.IsOnline = _chat.IsOnline(u.Id);
            u.IsMyFriend = true;

        }

        foreach (var u in friends
                        .OrderBy(u => !u.IsOnline)
                        .ThenBy(u => u.Login, StringComparer.OrdinalIgnoreCase))
        {
            _friendIds.Add(u.Id);
            Friends.Add(u);
            if (u.IsOnline)
                Online.Add(u);
        }
        var blocked = await _friends.GetBlockedAsync(_auth.CurrentUser.Id);
        foreach (var b in blocked.OrderBy(u => u.Login, StringComparer.OrdinalIgnoreCase))
            Blocked.Add(b);
    }

    private async Task LoadSuggestionsAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            IsSuggestOpen = false;
            return;
        }

        var list = await _sp.GetRequiredService<IProfileService>()
                            .FindByLoginAsync(SearchText!, take: 8);

        Suggestions.Clear();
        foreach (var u in list)
        {
            u.IsMyFriend = _friendIds.Contains(u.Id);
            Suggestions.Add(u);
        }

        IsSuggestOpen = Suggestions.Count > 0;
    }

    private void OnPresenceChanged(Guid userId, bool isOnline)
    {
        foreach (var u in Friends)
            if (u.Id == userId)
            {
                u.IsOnline = isOnline;
                if (isOnline)
                {
                    if (!Online.Contains(u)) Online.Add(u);
                }
                else
                {
                    Online.Remove(u);
                }
                break;
            }
    }
    public ObservableCollection<User> Suggestions { get; } = new();
}
