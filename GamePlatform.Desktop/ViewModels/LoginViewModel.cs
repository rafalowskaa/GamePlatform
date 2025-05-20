using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Validation;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly RegisterValidator _validator;

    public LoginViewModel(IAuthService auth, RegisterValidator val)
    {
        _auth = auth;
        _validator = val;
    }

    /* ─────────  pola  ───────── */
    [ObservableProperty] private bool _isRegistering;
    [ObservableProperty] private string _login = "";
    [ObservableProperty] private string _password = "";
    [ObservableProperty] private bool _showPassword;

    [ObservableProperty] private string _newLogin = "";
    [ObservableProperty] private string _email = "";
    public string RegPassword { get; set; } = "";
    public string RegPasswordRepeat { get; set; } = "";
    [ObservableProperty] private bool _showRegPassword;

    // -> NOWA właściwość do checkboxa „Zapamiętaj mnie”
    [ObservableProperty] private bool _rememberMe;

    [ObservableProperty] private string? _error;

    /* ─────────  helpers  ───────── */
    private void FlashError(string msg)
    {
        Error = msg;
        _ = Task.Delay(3000).ContinueWith(_ => Error = null);
    }

    private void ClearFields()
    {
        Password = "";
        RegPassword = RegPasswordRepeat = "";
    }

    /* ─────────  LOGOWANIE  ───────── */
    [RelayCommand]
    private async Task SubmitLogin(System.Windows.Window win)
    {
        if (await _auth.LoginAsync(Login, Password))
        {
            // TODO: RememberMe -> zapisz token/cookie etc.
            ClearFields();
            win.DialogResult = true;   // ZAMYKA okno modalne
        }
        else FlashError("Błędny login lub hasło.");
    }

    /* ─────────  REJESTRACJA  ───────── */
    [RelayCommand]
    private async Task SubmitRegister()
    {
        var dto = new RegisterDto(NewLogin, Email, RegPassword, RegPasswordRepeat);
        var res = _validator.Validate(dto);
        if (!res.IsValid) { FlashError(res.ToString("• ")); return; }

        if (await _auth.RegisterAsync(NewLogin, Email, RegPassword))
        {
            ClearFields();
            IsRegistering = false;
            FlashError("Konto utworzone – zaloguj się.");
        }
        else FlashError("Login lub e-mail jest już zajęty.");
    }
}
