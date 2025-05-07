using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Validation;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>
/// Jeden VM obsługujący zarówno logowanie, jak i rejestrację.
/// </summary>
public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly RegisterValidator _regValidator;

    public LoginViewModel(IAuthService auth, RegisterValidator regValidator)
    {
        _auth = auth;
        _regValidator = regValidator;
    }

    /* ===== tryb: false – login / true – register ===== */
    [ObservableProperty] private bool _isRegistering;

    /* ------------------ LOGOWANIE ------------------ */
    [ObservableProperty] private string _login = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private bool _rememberMe;

    /* ---------------- REJESTRACJA ------------------ */
    [ObservableProperty] private string _newLogin = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    public string RegPassword { private get; set; } = string.Empty;
    public string RegPasswordRepeat { private get; set; } = string.Empty;

    /* ---------------- WSPÓLNE ---------------------- */
    [ObservableProperty] private string? _error;

    public event Action? Authenticated;              // sygnał dla widoku

    /* ================= COMMANDY ================= */

    [RelayCommand]
    private async Task SubmitLogin(Window? win)
    {
        Error = null;

        if (await _auth.LoginAsync(Login, Password))
        {
            ClearFields();
            Authenticated?.Invoke();
        }
        else
            Error = "Błędny login lub hasło.";
    }

    [RelayCommand]
    private async Task SubmitRegister(Window? win)
    {
        Error = null;

        var dto = new RegisterDto(NewLogin, Email, RegPassword, RegPasswordRepeat);
        var res = _regValidator.Validate(dto);
        if (!res.IsValid) { Error = res.ToString("• "); return; }

        if (await _auth.RegisterAsync(NewLogin, Email, RegPassword))
        {
            await _auth.LoginAsync(NewLogin, RegPassword); // auto-login
            ClearFields();
            Authenticated?.Invoke();
        }
        else
            Error = "Login lub e-mail jest już zajęty.";
    }

    /* ---------------- pomocnicze ------------------ */
    private void ClearFields()
    {
        Password = string.Empty;
        RegPassword = RegPasswordRepeat = string.Empty;
        Error = null;
    }
}
