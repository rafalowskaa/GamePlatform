using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>Edycja własnego profilu.</summary>
public partial class ProfileViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly IProfileService _profileSvc;

    public ProfileViewModel(IAuthService auth, IProfileService profileSvc)
    {
        _auth = auth;
        _profileSvc = profileSvc;

        var u = _auth.CurrentUser!;
        NewLogin = u.Login;
        NewEmail = u.Email;
        RecoveryMail = u.RecoveryMail;
        AvatarPath = u.AvatarPath;
        if (File.Exists(AvatarPath))
            Avatar = new BitmapImage(new System.Uri(Path.GetFullPath(AvatarPath)));
    }

    /* -------- pola edycyjne -------- */
    [ObservableProperty] private string _newLogin;
    [ObservableProperty] private string _newEmail;
    [ObservableProperty] private string _recoveryMail;
    [ObservableProperty] private string _newPassword = "";
    [ObservableProperty] private string _avatarPath = "";
    [ObservableProperty] private BitmapImage? _avatar;
    [ObservableProperty] private string? _error;

    /* -------- Pick avatar ---------- */
    [RelayCommand]
    private void PickAvatar()
    {
        var dlg = new OpenFileDialog { Filter = "Image|*.png;*.jpg" };
        if (dlg.ShowDialog() != true) return;

        AvatarPath = dlg.FileName;
        Avatar = new BitmapImage(new System.Uri(AvatarPath));
    }

    /* -------- SAVE ---------- */
    [RelayCommand]
    private async Task SaveAsync()
    {
        Error = null;

        using Stream? avatarStream =
            File.Exists(AvatarPath) ? File.OpenRead(AvatarPath) : null;

        var u = _auth.CurrentUser!;
        u.Login = NewLogin;
        u.Email = NewEmail;
        u.RecoveryMail = RecoveryMail;

        var ok = await _profileSvc.UpdateAsync(u,
                                               string.IsNullOrWhiteSpace(NewPassword) ? null : NewPassword,
                                               avatarStream);

        if (!ok) Error = "Nie udało się zapisać profilu (duplikat loginu/e-maila).";
        else NewPassword = "";
    }

}
