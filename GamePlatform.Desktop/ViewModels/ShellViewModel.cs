using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GamePlatform.Desktop.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    private readonly MainViewModel _main;
    private readonly ProfileViewModel _profile;
    private readonly UsersViewModel _users;
    private readonly FriendListViewModel _friends;
    private readonly StatsViewModel _stats;
    private readonly IAuthService _auth;

    public ShellViewModel(IServiceProvider sp, IAuthService auth)
    {
        _main = sp.GetRequiredService<MainViewModel>();
        _profile = sp.GetRequiredService<ProfileViewModel>();
        _users = sp.GetRequiredService<UsersViewModel>();
        _friends = sp.GetRequiredService<FriendListViewModel>();
        _stats = sp.GetRequiredService<StatsViewModel>();
        _auth = auth;

        SetPage(new GamesPage { DataContext = _main }, "Games");
    }

    /* ---------------- bindings ---------------- */
    [ObservableProperty] private Page _currentPage = null!;
    [ObservableProperty] private string _currentPageTag = "Games";

    public bool IsAdmin => _auth.CurrentRole == UserRole.Admin;

    private void SetPage(Page page, string tag)
    {
        CurrentPage = page;
        CurrentPageTag = tag;
    }

    /* --------------- commands ----------------- */
    [RelayCommand] private void ShowGames() => SetPage(new GamesPage { DataContext = _main }, "Games");
    [RelayCommand] private void ShowProfile() => SetPage(new ProfilePage { DataContext = _profile }, "Profile");
    [RelayCommand] private void ShowFriends() => SetPage(new FriendsPage { DataContext = _friends }, "Friends");
    [RelayCommand] private void ShowStats() => SetPage(new StatsPage { DataContext = _stats }, "Stats");
    [RelayCommand]
    private void ShowUsers()
    { if (IsAdmin) SetPage(new UsersPage { DataContext = _users }, "Users"); }

    /* --------------- logout ------------------- */
    [RelayCommand]
    private async void Logout()
    {
        await _auth.LogoutAsync();
        OnPropertyChanged(nameof(IsAdmin));        // odśwież przycisk „Użytkownicy”
        // zamykamy główne okno powłoki -> wracamy do pętli w App.xaml.cs
        Application.Current.MainWindow?.Close();
    }
}
