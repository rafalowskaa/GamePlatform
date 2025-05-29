using System.IO;
using System.Threading.Tasks;
using GamePlatform.Shared.Models;

namespace GamePlatform.Desktop.Services;

/// <summary>
/// Edycja profilu zalogowanego użytkownika (login, e-mail, hasło, avatar).
/// </summary>
public interface IProfileService
{
    /// <param name="updated">obiekt z nowymi wartościami Login / Email / RecoveryMail</param>
    /// <param name="newPassword"> nowe hasło (null/empty → bez zmian)</param>
    /// <param name="newAvatar"> strumień pliku graficznego (null → bez zmian)</param>
    Task<bool> UpdateAsync(User updated, string? newPassword, Stream? newAvatar);

    /// <summary>
    /// Szukanie użytkowników po prefiksie loginu. Wynik ograniczony parametrem
    /// <paramref name="take"/>.
    /// </summary>
    Task<IReadOnlyList<User>> FindByLoginAsync(string prefix, int take = 20);
}
