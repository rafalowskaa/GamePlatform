using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;

namespace GamePlatform.Desktop.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    private readonly IServiceProvider _sp;
    private readonly IAuthService _auth;
    private readonly FriendListViewModel _friendsVm;
    public FriendListViewModel FriendsVm => _friendsVm;

    public ShellViewModel(IServiceProvider sp, IAuthService auth)
    {
        _sp = sp;
        _auth = auth;

        
        _friendsVm = _sp.GetRequiredService<FriendListViewModel>();
        

        ShowGames();                                  // strona domyślna
    }

    /* ==========  BINDS  ========== */
    [ObservableProperty] private Page _currentPage = null!;
    [ObservableProperty] private string _currentTag = "";
    [ObservableProperty] private bool _isDrawerOpen;

 
    public bool IsAdmin => _auth.CurrentRole is UserRole.Admin or UserRole.Moderator;

    /* ==========  NAWIGACJA  ========== */
    private void Navigate(Page page, string tag)
    {
        CurrentPage = page;
        CurrentTag = tag;
    }

    [RelayCommand] private void ShowGames() => Navigate(new GamesPage { DataContext = _sp.GetRequiredService<MainViewModel>() }, "Games");
    [RelayCommand] private void ShowProfile() => Navigate(new ProfilePage { DataContext = _sp.GetRequiredService<ProfileViewModel>() }, "Profile");
    [RelayCommand] private void ShowFriends() => Navigate(new FriendsPage { DataContext = _friendsVm }, "Friends");
    [RelayCommand] private void ToggleDrawer() => IsDrawerOpen = !IsDrawerOpen;
    [RelayCommand] private void ShowStats() => Navigate(new StatsPage { DataContext = _sp.GetRequiredService<StatsViewModel>() }, "Stats");
    [RelayCommand]
    private void ShowUsers()
    {
        if (IsAdmin)
            Navigate(new UsersPage { DataContext = _sp.GetRequiredService<UsersViewModel>() }, "Users");
    }


    /* ==========  LOGOUT  ========== */
    [RelayCommand]
    private async void Logout()
    {
        await _auth.LogoutAsync();
        OnPropertyChanged(nameof(IsAdmin));
        System.Windows.Application.Current.MainWindow?.Close();
    }
}
