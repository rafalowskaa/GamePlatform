// ViewModels/LoginViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Validation;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace GamePlatform.Desktop.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly RegisterValidator _validator;

    public LoginViewModel(IAuthService auth, RegisterValidator validator)
    {
        _auth = auth;
        _validator = validator;
    }

    /* --------  tryb  (false = login, true = register) -------- */
    [ObservableProperty] private bool _isRegistering;

    /* --------  LOGOWANIE  -------- */
    [ObservableProperty] private string _login = string.Empty;
    [ObservableProperty] private string _password = string.Empty;

    /* --------  REJESTRACJA  -------- */
    [ObservableProperty] private string _newLogin = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    public string RegPassword { private get; set; } = string.Empty;
    public string RegPasswordRepeat { private get; set; } = string.Empty;

    /* --------  wspólne  -------- */
    [ObservableProperty] private string? _error;

    /* ======  Zdarzenie, które przechwytuje LoginWindow  ====== */
    public event Action? Authenticated;

    /* ==========================================================
     *  COMMANDY
     * =========================================================*/
    [RelayCommand]
    private async Task SubmitLogin(Window? _)
    {
        Error = null;

        if (await _auth.LoginAsync(Login, Password))
        {
            ClearFields();
            Authenticated?.Invoke();            // powiadamiamy View
        }
        else
            Error = "Błędny login lub hasło.";
    }

    [RelayCommand]
    private async Task SubmitRegister(Window? _)
    {
        Error = null;

        var dto = new RegisterDto(NewLogin, Email, RegPassword, RegPasswordRepeat);
        var res = _validator.Validate(dto);
        if (!res.IsValid)
        {
            Error = res.ToString("• ");
            return;
        }

        if (await _auth.RegisterAsync(NewLogin, Email, RegPassword))
        {
            await _auth.LoginAsync(NewLogin, RegPassword);  // auto-login
            ClearFields();
            Authenticated?.Invoke();
        }
        else
            Error = "Login lub e-mail już istnieje.";
    }

    /* pomocnicze */
    private void ClearFields()
    {
        Password = string.Empty;
        RegPassword = string.Empty;
        RegPasswordRepeat = string.Empty;
        Error = null;
    }
}
