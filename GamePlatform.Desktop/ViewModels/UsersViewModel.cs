using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>Lista wszystkich użytkowników – dostępna tylko dla Admina.</summary>
public partial class UsersViewModel : ObservableObject
{
    private readonly AppDbContext _db;

    public UsersViewModel(AppDbContext db)
    {
        _db = db;
        _ = LoadAsync();
    }

    public ObservableCollection<User> Users { get; } = new();

    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string? _error;

    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;
        Users.Clear();
        foreach (var u in await _db.Users.AsNoTracking().ToListAsync())
            Users.Add(u);
        IsBusy = false;
    }

    [RelayCommand]
    private async Task ChangeRoleAsync(User? u)
    {
        if (u is null) return;
        var tracked = await _db.Users.FindAsync(u.Id);
        if (tracked is null) return;

        tracked.Role = tracked.Role switch
        {
            UserRole.Guest => UserRole.User,
            UserRole.User => UserRole.Admin,
            UserRole.Admin => UserRole.User,
            _ => UserRole.User
        };
        await _db.SaveChangesAsync();
        await LoadAsync();
    }

    [RelayCommand]
    private async Task DeleteAsync(User? u)
    {
        if (u is null || u.Role == UserRole.Admin) return;
        _db.Users.Remove(u);
        await _db.SaveChangesAsync();
        Users.Remove(u);
    }
}
