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
/// </summary>
public partial class EditUserViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private Guid _userId;

    public EditUserViewModel(AppDbContext db)
    {
        _db = db;

        /*  pełna lista ról – Admin | Moderator | User  */
        Roles = new[]
        {
            UserRole.Admin,
            UserRole.Moderator,
            UserRole.User
        };
    }

    /* ----------  PUBLICZNE KOLEKCJE  ---------- */
    public UserRole[] Roles { get; }

    /* ----------  WŁAŚCIWOŚCI  ---------- */
    [ObservableProperty] private string _login = "";
    [ObservableProperty] private string _email = "";
    [ObservableProperty] private UserRole _role;
    [ObservableProperty] private DateTime? _blockedUntilUtc;
    [ObservableProperty] private string? _blockReasonPl;
    [ObservableProperty] private string? _blockReasonEn;

    /* ----------  INICJALIZACJA  ---------- */
    public void Init(User src)
    {
        _userId = src.Id;
        Login = src.Login;
        Email = src.Email;
        Role = src.Role;
        BlockedUntilUtc = src.BlockedUntilUtc;
        BlockReasonPl = src.BlockReason;
        BlockReasonEn = src.BlockReason;
    }

    /* ----------  KOMENDY  ---------- */

    /// <summary>Zdjęcie blokady (przycisk ✕ obok DatePickera).</summary>
    [RelayCommand]
    private void ClearBlock() => BlockedUntilUtc = null;

    /// <summary>Zapisz zmiany i zamknij okno (DialogResult = true).</summary>
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
        u.BlockReason = BlockReasonPl;

        await _db.SaveChangesAsync();
        win.DialogResult = true;    // zamyka okno z rezultatem OK
    }
}
