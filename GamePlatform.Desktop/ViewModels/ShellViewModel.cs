using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Enums;
using System.Windows.Controls;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>
/// Główna „skorupa” – przełącza strony i obsługuje wylogowanie.
/// </summary>
public partial class ShellViewModel : ObservableObject
{
    private readonly MainViewModel _main;
    private readonly ProfileViewModel _profile;
    private readonly UsersViewModel _users;
    private readonly FriendListViewModel _friends;
    private readonly IAuthService _auth;

    public ShellViewModel(MainViewModel main,
                          ProfileViewModel profile,
                          UsersViewModel users,
                          FriendListViewModel friends,
                          IAuthService auth)
    {
        _main = main;
        _profile = profile;
        _users = users;
        _friends = friends;
        _auth = auth;

        CurrentPage = new GamesPage { DataContext = _main };
    }

    [ObservableProperty] private Page _currentPage = null!;

    public bool IsAdmin => _auth.CurrentRole == UserRole.Admin;

    /* ---------- NAWIGACJA ---------- */
    [RelayCommand] private void ShowGames() => CurrentPage = new GamesPage { DataContext = _main };
    [RelayCommand] private void ShowProfile() => CurrentPage = new ProfilePage { DataContext = _profile };
    [RelayCommand] private void ShowFriends() => CurrentPage = new FriendsPage { DataContext = _friends };

    [RelayCommand]
    private void ShowUsers()
    {
        if (IsAdmin)
            CurrentPage = new UsersPage { DataContext = _users };
    }

    /* ---------- WYLOGOWANIE ---------- */
    [RelayCommand]
    private async void Logout()
    {
        await _auth.LogoutAsync();
        System.Windows.Application.Current.Shutdown();   // restart aplikacji = ponowne logowanie
    }
}
