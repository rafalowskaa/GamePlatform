using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>VM wyszukiwarki nowych znajomych.</summary>
public partial class FindFriendsViewModel : ObservableObject
{
    private readonly IFriendService _friends;
    private readonly IAuthService _auth;

    public FindFriendsViewModel(IFriendService fs, IAuthService auth)
    {
        _friends = fs;
        _auth = auth;
    }

    public ObservableCollection<User> Results { get; } = new();

    [ObservableProperty] private string _searchText = "";

    /* ENTER w TextBoxie lub klik przycisku */
    [RelayCommand]
    private async Task SearchAsync()
    {
        Results.Clear();
        if (string.IsNullOrWhiteSpace(SearchText)) return;

        var list = await _friends.FindUsersAsync(SearchText, _auth.CurrentUser!.Id);
        foreach (var u in list) Results.Add(u);
    }

    [RelayCommand]
    private async Task InviteAsync(User? u)
    {
        if (u is null) return;
        await _friends.SendRequestAsync(_auth.CurrentUser!.Id, u.Id);
        Results.Remove(u);   // znika z listy po wysłaniu zaproszenia
    }
}
