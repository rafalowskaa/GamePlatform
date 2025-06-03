using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.Utils;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Loc = GamePlatform.Desktop.Localization.Localization;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>Edycja własnego profilu.</summary>
public partial class ProfileViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly IProfileService _profileSvc;
    private readonly IGameRepository _games;
    private readonly INotificationService _notify;

    public ProfileViewModel(IAuthService auth, IProfileService profileSvc, IGameRepository games, INotificationService notify)
    {
        _auth = auth;
        _profileSvc = profileSvc;
        _games = games;
        _notify = notify;

        var u = _auth.CurrentUser!;
        NewLogin = u.Login;
        NewEmail = u.Email;
        RecoveryMail = u.RecoveryMail;
        AvatarPath = u.AvatarPath;
        if (File.Exists(AvatarPath))
            Avatar = new BitmapImage(new System.Uri(Path.GetFullPath(AvatarPath)));

        CurrentXp = u.Xp;
        NextXp = RankHelper.GetNextXp(CurrentXp);
        RankEnum = RankHelper.GetRank(CurrentXp);
        NextRankEnum = RankHelper.GetNextRank(CurrentXp);
        Rank = RankEnum.ToString();
        NextRank = NextRankEnum.ToString();
        _ = LoadFavoritesAsync();
    }

    /* -------- pola edycyjne -------- */
    [ObservableProperty] private string _newLogin;
    [ObservableProperty] private string _newEmail;
    [ObservableProperty] private string _recoveryMail;
    [ObservableProperty] private string _newPassword = "";
    [ObservableProperty] private string _avatarPath = "";
    [ObservableProperty] private BitmapImage? _avatar;
    [ObservableProperty] private string? _error;

    [ObservableProperty] private ObservableCollection<Game> _favorites = new();
    [ObservableProperty] private int _currentXp;
    [ObservableProperty] private int _nextXp;

    
    public string Rank { get; }
    public UserRank RankEnum { get; }
    public UserRank NextRankEnum { get; }
    public string NextRank { get; }

    /* -------- Commands for favorites -------- */
    [RelayCommand]
    private async Task ToggleFavoriteAsync(Game? g)
    {
        if (g is null) return;
        await _games.ToggleFavoriteAsync(_auth.CurrentUser!.Id, g.Id);
        await LoadFavoritesAsync();
    }

    [RelayCommand]
    private void Play(Game? g)
    {
        if (g is null) return;

        var latest = g.Versions.OrderByDescending(v => v.Number).FirstOrDefault();
        if (latest is null) return;

        string? html = Directory.GetFiles(latest.FolderPath, "index.html", SearchOption.AllDirectories).FirstOrDefault();
        if (html is not null)
        {
            var start = DateTime.UtcNow;
            var win = new PlayWindow
            {
                DataContext = new PlayWindowViewModel(g, html),
                Owner = System.Windows.Application.Current.MainWindow
            };
            win.Closed += async (_, __) =>
            {
                var seconds = (int)(DateTime.UtcNow - start).TotalSeconds;
                await _games.AddPlayTimeAsync(_auth.CurrentUser!.Id, g.Id, seconds);
            };
            win.Show();
        }
        else
        {
            string? exe = Directory.GetFiles(latest.FolderPath, "*.exe", SearchOption.AllDirectories).FirstOrDefault();
            if (exe is not null)
            {
                try
                {
                    var psi = new System.Diagnostics.ProcessStartInfo(exe)
                    {
                        UseShellExecute = true,
                        WorkingDirectory = Path.GetDirectoryName(exe)
                    };
                    var proc = System.Diagnostics.Process.Start(psi);
                    if (proc != null)
                    {
                        var start = DateTime.UtcNow;
                        proc.EnableRaisingEvents = true;
                        proc.Exited += async (_, __) =>
                        {
                            var seconds = (int)(DateTime.UtcNow - start).TotalSeconds;
                            await _games.AddPlayTimeAsync(_auth.CurrentUser!.Id, g.Id, seconds);
                        };
                    }
                }
                catch { }
            }
        }
    }

    /* -------- Pick avatar ---------- */
    [RelayCommand]
    private void PickAvatar()
    {
        var dlg = new OpenFileDialog { Filter = "Image|*.png;*.jpg" };
        if (dlg.ShowDialog() != true) return;

        AvatarPath = dlg.FileName;
        Avatar = new BitmapImage(new System.Uri(AvatarPath));
    }

    [RelayCommand]
    private void EditInfo()
    {
        var dlg = new EditProfileDialog { DataContext = this, Owner = System.Windows.Application.Current.MainWindow };
        dlg.ShowDialog();
    }

    /* -------- SAVE ---------- */
    [RelayCommand]
    private async Task SaveAsync()
    {
        Error = null;

        var pwdDlg = new PasswordPromptWindow { Owner = System.Windows.Application.Current.MainWindow };
        if (pwdDlg.ShowDialog() != true) return;

        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
        if (hasher.VerifyHashedPassword(_auth.CurrentUser!, _auth.CurrentUser!.PasswordHash, pwdDlg.Password)
            == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
        {
            System.Windows.MessageBox.Show(Loc.TRaw("INVALID_PASSWORD"), Loc.TRaw("ERROR"), System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            return;
        }


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
        else
        {
            NewPassword = "";
            _notify.Show(Loc.TRaw("PROFILE_UPDATED"));
        }
    }

    private async Task LoadFavoritesAsync()
    {
        var list = await _games.GetFavoritesAsync(_auth.CurrentUser!.Id);
        Favorites = new ObservableCollection<Game>(list);
    }

}
