using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>
/// VM okna „Edytuj użytkownika”.
/// Wszystkie właściwości są zdefiniowane ręcznie; nie używamy atrybutów
/// Source-Generatora, więc nie powstaną żadne duplikaty.
/// </summary>
public partial class EditUserViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private Guid _userId;

    public EditUserViewModel(AppDbContext db) => _db = db;

    /* ────────── Właściwości ────────── */

    private string _login = "";
    public string Login
    {
        get => _login;
        private set => SetProperty(ref _login, value);
    }

    private string _email = "";
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private UserRole _role;
    public UserRole Role
    {
        get => _role;
        set => SetProperty(ref _role, value);
    }

    private DateTime? _blockedUntilUtc;
    public DateTime? BlockedUntilUtc
    {
        get => _blockedUntilUtc;
        set => SetProperty(ref _blockedUntilUtc, value);
    }

    private string? _blockReason;
    public string? BlockReason
    {
        get => _blockReason;
        set => SetProperty(ref _blockReason, value);
    }

    /// <summary>Przenosi dane wybranego wiersza do pól dialogu.</summary>
    public void Init(User src)
    {
        _userId = src.Id;
        Login = src.Login;
        Email = src.Email;
        Role = src.Role;
        BlockedUntilUtc = src.BlockedUntilUtc;
        BlockReason = src.BlockReason;
    }

    /* ────────── Zapis ────────── */

    [RelayCommand]
    private async Task Save(System.Windows.Window win)
    {
        var u = await _db.Users.FirstOrDefaultAsync(x => x.Id == _userId);
        if (u is null)
        {
            win.Close();
            return;
        }

        u.Email = Email;
        u.Role = Role;
        u.BlockedUntilUtc = BlockedUntilUtc;
        u.BlockReason = BlockReason;

        await _db.SaveChangesAsync();
        win.DialogResult = true;     // zamyka okno z rezultatem OK
    }
}
