using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.Views;
using GamePlatform.Desktop.Localization;
using GamePlatform.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using Loc = GamePlatform.Desktop.Localization.Localization;

namespace GamePlatform.Desktop.ViewModels;

public partial class ShellViewModel : ObservableObject
{
    private readonly IServiceProvider _sp;
    private readonly IAuthService _auth;
    private readonly IChatService _chat;
    private readonly FriendListViewModel _friendsVm;
    private readonly INotificationService _notify;
    public FriendListViewModel FriendsVm => _friendsVm;

    public ShellViewModel(IServiceProvider sp, IAuthService auth)
    {
        _sp = sp;
        _auth = auth;
        _chat = _sp.GetRequiredService<IChatService>();
        _notify = _sp.GetRequiredService<INotificationService>();

        // update texts when language changes
        Localization.LanguagePropertyChanged += OnLocalizationPropertyChanged;


        _friendsVm = _sp.GetRequiredService<FriendListViewModel>();

        if (_auth.CurrentUser is not null)
            _ = _chat.SetOnlineAsync(_auth.CurrentUser.Id);


        ShowGames();                                  // strona domyślna
    }

    /* ==========  BINDS  ========== */
    [ObservableProperty] private object _currentPage = null!;
    [ObservableProperty] private string _currentTag = "";
    [ObservableProperty] private bool _isDrawerOpen;
    [ObservableProperty] private int _selectedTabIndex;

    public string WelcomeText => string.Format(Loc.TRaw("WELCOME_PLAYER"), _auth.CurrentLogin);
    public string? AvatarPath => _auth.CurrentUser?.AvatarPath;
    public string? CurrentLogin => _auth.CurrentLogin;




    public bool IsAdmin => _auth.CurrentRole is UserRole.Admin or UserRole.Moderator;

    /* ==========  NAWIGACJA  ========== */
    private void Navigate(object page, string tag)
    {
        CurrentPage = page;
        CurrentTag = tag;
        SelectedTabIndex = tag switch
        {
            "Games" => 0,
            "Profile" => 1,
            "Stats" => 2,
            "Notify" => 3,
            "Pending" => 4,
            "Reports" => 5,
            "Users" => 6,
            _ => SelectedTabIndex
        };
    }

    [RelayCommand] private void ShowGames() => Navigate(new GamesPage { DataContext = _sp.GetRequiredService<MainViewModel>() }, "Games");
    [RelayCommand] private void ShowProfile() => Navigate(new ProfilePage { DataContext = _sp.GetRequiredService<ProfileViewModel>() }, "Profile");
    [RelayCommand] private void ShowFriends() => Navigate(new FriendsPage { DataContext = _friendsVm }, "Friends");
    [RelayCommand] private void ToggleDrawer() => IsDrawerOpen = !IsDrawerOpen;
    [RelayCommand] private void ShowStats() => Navigate(new StatsPage { DataContext = _sp.GetRequiredService<StatsViewModel>() }, "Stats");
    [RelayCommand]
    private void ShowPending()
    {
        if (IsAdmin)
            Navigate(new AdminReviewPage { DataContext = _sp.GetRequiredService<AdminReviewViewModel>() }, "Pending");
    }
    [RelayCommand]
    private void ShowUsers()
    {
        if (IsAdmin)
            Navigate(new UsersPage { DataContext = _sp.GetRequiredService<UsersViewModel>() }, "Users");
    }

    [RelayCommand]
    private void ShowReports()
    {
        if (IsAdmin)
            Navigate(new ReportsPage { DataContext = _sp.GetRequiredService<ReportsViewModel>() }, "Reports");
    }

    [RelayCommand]
    private void ShowNotifications()
       => Navigate(new NotificationCenterPage { DataContext = _notify }, "Notify");

    [RelayCommand]
    private void ToggleLanguage()
    {
        Loc.Current = Loc.Current == Lang.Polish ? Lang.English : Lang.Polish;
    }

    [RelayCommand]
    private void SetPolish()
        => Loc.Current = Lang.Polish;

    [RelayCommand]
    private void SetEnglish()
       => Loc.Current = Lang.English;


    /* ==========  LOGOUT  ========== */
    [RelayCommand]
    private async void Logout()
    {
        var id = _auth.CurrentUser?.Id;
        if (id is not null)
            await _chat.SetOfflineAsync(id.Value);

        _friendsVm.Dispose();
        await _auth.LogoutAsync();
        OnPropertyChanged(nameof(IsAdmin));
        System.Windows.Application.Current.MainWindow?.Close();
    }

    private void OnLocalizationPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Localization.Current))
            OnPropertyChanged(nameof(WelcomeText));
    }
}
