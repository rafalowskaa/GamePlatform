using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.ViewModels;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Services;
using GamePlatform.Shared.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GamePlatform.Desktop;

/// <summary>
/// Główna klasa aplikacji.
/// </summary>
public partial class App : Application
{
    public IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        /* ------- globalny catcher -------- */
        AppDomain.CurrentDomain.UnhandledException += (_, a)
            => ShowFatal((Exception)a.ExceptionObject);

        DispatcherUnhandledException += (_, a) =>
        {
            ShowFatal(a.Exception);
            a.Handled = true;                 // nie zamykaj procesu
        };

        /* ---------- DI / EF ---------- */
        var sc = new ServiceCollection();

        sc.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=games.db"));

        sc.AddScoped<IAuthService, AuthService>();
        sc.AddScoped<IGameRepository, GameRepositorySqlite>();
        sc.AddScoped<IProfileService, ProfileService>();
        sc.AddScoped<IFriendService, FriendService>();
        sc.AddSingleton<INotificationService, NotificationService>();

        sc.AddScoped<GameUploadValidator>();
        sc.AddScoped<RegisterValidator>();
        sc.AddScoped<UserValidator>();

        sc.AddTransient<LoginViewModel>();
        sc.AddTransient<MainViewModel>();
        sc.AddTransient<UsersViewModel>();
        sc.AddTransient<ProfileViewModel>();
        sc.AddTransient<FriendListViewModel>();
        sc.AddTransient<StatsViewModel>();
        sc.AddTransient<AddGameDialogViewModel>();
        sc.AddTransient<ShellViewModel>();

        Services = sc.BuildServiceProvider();

        /* ---------- migracja + admin ---------- */
        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();

            if (!db.Users.Any())
            {
                db.Users.Add(new User
                {
                    Login = "admin",
                    Email = "admin@demo.dev",
                    Role = UserRole.Admin,
                    PasswordHash = new PasswordHasher<User>()
                                   .HashPassword(null!, "admin")
                });
                db.SaveChanges();
            }
        }

        /* ---------- okno-keeper ---------- */
        var keeper = MakeKeeper();
        MainWindow = keeper;
        keeper.Show();

        /* ---------- pętla ---------- */
        while (true)
        {
            if (!ShowLogin()) { Shutdown(); return; }

            // --- UWAGA: najpierw tworzymy i ustawiamy nowe MainWindow,
            //     a dopiero potem zamykamy stare, żeby WPF nie wywołał Shutdown().

            var shell = new ShellWindow(
                            Services.GetRequiredService<ShellViewModel>());

            MainWindow = shell;       // <- teraz Shell jest głównym oknem
            if (keeper.IsVisible)     //    więc zamknięcie keepera nic nie wywoła
                keeper.Close();

            shell.ShowDialog();       // ← blokuje aż do zamknięcia powłoki

            /* jeżeli Shell zamknął się „Wyloguj” → CurrentRole = Guest
             * jeżeli [X] przy zalogowanym userze → inna rola => Exit   */
            if (Services.GetRequiredService<IAuthService>().CurrentRole
                != UserRole.Guest)
            {
                Shutdown();                                // [X]
                return;
            }

            // Logout → start od nowa
            keeper = MakeKeeper();
            MainWindow = keeper;
            keeper.Show();
        }
    }

    /* ---------- helpers ---------- */

    private static Window MakeKeeper() => new()
    {
        Width = 0,
        Height = 0,
        Opacity = 0,
        ShowInTaskbar = false,
        WindowStyle = WindowStyle.None
    };

    private bool ShowLogin()
    {
        var win = new LoginWindow
        {
            DataContext = Services.GetRequiredService<LoginViewModel>()
        };
        return win.ShowDialog() == true;
    }

    private static void ShowFatal(Exception ex)
        => MessageBox.Show(ex.ToString(), "Błąd krytyczny",
                           MessageBoxButton.OK, MessageBoxImage.Error);
}
