using System.Threading.Tasks;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;

namespace GamePlatform.Desktop.Services;

/// <summary>
/// Autoryzacja, rejestracja i bieżący kontekst użytkownika.
/// </summary>
public interface IAuthService
{
    /* BIEŻĄCY KONTEKST ---------------------------------------------------- */
    User? CurrentUser { get; }
    UserRole CurrentRole { get; }
    string? CurrentLogin { get; }

    /* OPERACJE ----------------------------------------------------------- */
    //Task<bool> LoginAsync(string loginOrEmail, string password);
    Task<bool> RegisterAsync(string login, string email, string password);
    Task<LoginResult> LoginAsync(string loginOrEmail, string password);

    Task LogoutAsync();
}
