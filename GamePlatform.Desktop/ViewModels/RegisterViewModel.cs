using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using GamePlatform.Desktop.Services;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly IValidator<RegisterViewModel> _validator;

    public RegisterViewModel(IAuthService auth, IValidator<RegisterViewModel> v)
    {
        _auth = auth;
        _validator = v;
    }

    [ObservableProperty] private string login = string.Empty;
    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string? error;

    [RelayCommand]
    private async Task<bool> SubmitAsync()
    {
        Error = null;
        var res = _validator.Validate(this);
        if (!res.IsValid)
        {
            Error = res.ToString("• ");
            return false;
        }

        if (await _auth.RegisterAsync(Login, Email, Password))
            return true;

        Error = "Login lub e-mail jest już zajęty.";
        return false;
    }
}
