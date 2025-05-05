using GamePlatform.Shared.Enums;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.Services;

public interface IAuthService
{
    UserRole CurrentRole { get; }
    string? CurrentLogin { get; }

    Task<bool> LoginAsync(string loginOrMail, string password);
    Task LogoutAsync();
    Task<bool> RegisterAsync(string login, string email, string password);
}
