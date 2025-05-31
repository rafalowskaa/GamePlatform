// ViewModels/PendingRequestsViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.DTO;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GamePlatform.Desktop.ViewModels;

public partial class PendingRequestsViewModel : ObservableObject
{
    private readonly IFriendService _friends;
    private readonly IAuthService _auth;
    private readonly FriendListViewModel _friendsVm;

    public PendingRequestsViewModel(IFriendService fs, IAuthService au, FriendListViewModel fl)
    {
        _friends = fs;
        _auth = au;
        _friendsVm = fl;

        _ = RefreshAsync();
    }

    public ObservableCollection<FriendRequestDto> Requests { get; } = new();

    [RelayCommand]
    private async Task RefreshAsync()
    {
        Requests.Clear();
        foreach (var r in await _friends.GetPendingAsync(_auth.CurrentUser!.Id))
            Requests.Add(r);
    }

    [RelayCommand]
    private async Task AcceptAsync(FriendRequestDto? r)
    {
        if (r is null) return;
        await _friends.AcceptRequestAsync(r.Id);
        await RefreshAsync();
        await _friendsVm.RefreshFriendsAsync();
    }

    [RelayCommand]
    private async Task DeclineAsync(FriendRequestDto? r)
    {
        if (r is null) return;
        await _friends.DeclineRequestAsync(r.Id);
        await RefreshAsync();
        await _friendsVm.RefreshFriendsAsync();
    }
}
