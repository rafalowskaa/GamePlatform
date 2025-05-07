using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

public partial class FriendListViewModel : ObservableObject
{
    private readonly IFriendService _friends;
    private readonly IAuthService _auth;

    public FriendListViewModel(IFriendService fs, IAuthService auth)
    {
        _friends = fs;
        _auth = auth;
        _ = RefreshAsync();
    }

    public ObservableCollection<User> Friends { get; } = new();

    [ObservableProperty] private bool _isBusy;

    [RelayCommand]
    private async Task RefreshAsync()
    {
        if (_auth.CurrentUser is null) return;
        IsBusy = true;
        Friends.Clear();
        foreach (var u in await _friends.GetFriendsAsync(_auth.CurrentUser.Id))
            Friends.Add(u);
        IsBusy = false;
    }

    [RelayCommand]
    private async Task RemoveAsync(User? u)
    {
        if (u is null || _auth.CurrentUser is null) return;
        await _friends.RemoveFriendAsync(_auth.CurrentUser.Id, u.Id);
        await RefreshAsync();
    }
}
