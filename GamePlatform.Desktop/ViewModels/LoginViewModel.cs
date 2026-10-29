using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Validation;
using System.Threading.Tasks;
using System.IO;
using GamePlatform.Desktop.Localization;

using Loc = GamePlatform.Desktop.Localization.Localization;

namespace GamePlatform.Desktop.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly RegisterValidator _validator;
    private readonly INotificationService _notify;

    private static readonly string RememberPath = System.IO.Path.Combine(
        System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData),
        "GamePlatform", "remember.txt");

    public LoginViewModel(IAuthService auth, RegisterValidator val, INotificationService notify)
    {
        _auth = auth;
        _validator = val;
        _notify = notify;


        _ = LoadRememberedLoginAsync();
    }

    private async Task LoadRememberedLoginAsync()
    {
        try
        {
            if (System.IO.File.Exists(RememberPath))
            {
                Login = await System.IO.File.ReadAllTextAsync(RememberPath);
                RememberMe = true;
            }
        }
        catch { }
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
        /*  nowy wynik = obiekt  */
        var result = await _auth.LoginAsync(Login, Password);

        if (result.IsSuccess)
        {
            // Remember the login name only; passwords and sessions are not persisted.
            try
            {
                if (RememberMe)
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(RememberPath)!);
                    System.IO.File.WriteAllText(RememberPath, Login);
                }
                else if (System.IO.File.Exists(RememberPath))
                {
                    System.IO.File.Delete(RememberPath);
                }
            }
            catch { }

            if (_auth.CurrentUser is not null)
                _notify.LoadForUser(_auth.CurrentUser.Id);

            ClearFields();
            win.DialogResult = true;   // zamyka okno modalne
        }
        else
        {
            FlashError(result.Error ?? "Logowanie nieudane");
        }
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


    /* ─────────  JĘZYK  ───────── */
    [RelayCommand]
    private void SetPolish() => Loc.Current = Lang.Polish;

    [RelayCommand]
    private void SetEnglish() => Loc.Current = Lang.English;
}
