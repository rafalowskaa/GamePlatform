using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private readonly IAuthService _auth;

    public UsersViewModel(AppDbContext db, IAuthService auth)
    { _db = db; _auth = auth; _ = LoadAsync(); }

    public ObservableCollection<User> Users { get; } = new();

    [ObservableProperty] private bool _isBusy;

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        Users.Clear();
        foreach (var u in await _db.Users.AsNoTracking().ToListAsync())
            Users.Add(u);
        IsBusy = false;
    }

    /* --------------- SAVE ---------------- */
    [RelayCommand]
    private async Task SaveAsync(object? p)
    {
        if (p is not User u) return;
        if (_auth.CurrentRole == UserRole.Moderator && u.Role == UserRole.Admin) return;

        var tracked = await _db.Users.FindAsync(u.Id);
        if (tracked is null) return;

        tracked.Login = u.Login;
        tracked.Email = u.Email;
        tracked.Role = u.Role;
        tracked.BlockedUntilUtc = u.BlockedUntilUtc;
        tracked.BlockReason = u.BlockReason;

        await _db.SaveChangesAsync();
    }

    /* --------------- DELETE -------------- */
    [RelayCommand]
    private async Task DeleteAsync(object? p)
    {
        if (p is not User u) return;

        if (_auth.CurrentRole == UserRole.Moderator && u.Role is UserRole.Admin or UserRole.Moderator) return;
        if (_auth.CurrentUser?.Id == u.Id) return;

        _db.Users.Remove(u);
        await _db.SaveChangesAsync();
        Users.Remove(u);
    }
}
