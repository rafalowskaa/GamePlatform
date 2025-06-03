using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using GamePlatform.Shared.Enums;
using System.Linq;
using GamePlatform.Desktop.Views;
using System.Diagnostics;

namespace GamePlatform.Desktop.ViewModels;

/// <summary>Edycja własnego profilu.</summary>
public partial class ProfileViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly IProfileService _profileSvc;
    private readonly IGameRepository _games;

    public ProfileViewModel(IAuthService auth, IProfileService profileSvc, IGameRepository games)
    {
        _auth = auth;
        _profileSvc = profileSvc;
        _games = games;

        var u = _auth.CurrentUser!;
        NewLogin = u.Login;
        NewEmail = u.Email;
        RecoveryMail = u.RecoveryMail;
        AvatarPath = u.AvatarPath;
        if (File.Exists(AvatarPath))
            Avatar = new BitmapImage(new System.Uri(Path.GetFullPath(AvatarPath)));

        CurrentXp = u.Xp;
        NextXp = RankHelper.GetNextXp(CurrentXp);
        Rank = u.Rank.ToString();
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

    public int CurrentXp { get; }
    public int NextXp { get; }
    public string Rank { get; }

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

    private async Task LoadFavoritesAsync()
    {
        var list = await _games.GetFavoritesAsync(_auth.CurrentUser!.Id);
        Favorites = new ObservableCollection<Game>(list);
    }

}
