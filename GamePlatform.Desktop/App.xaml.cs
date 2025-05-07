//  App.xaml.cs
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
using System.Linq;
using System.Windows;

namespace GamePlatform.Desktop;

/// <summary>
///  Start-up aplikacji: DI → migracja EF → okno logowania → shell.
/// </summary>
public partial class App : Application
{
    public IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        /* ========== 1.  DI / EF / Walidatory / VM-y ========== */
        var sc = new ServiceCollection();

        // 1️⃣ baza
        sc.AddDbContext<AppDbContext>(
            o => o.UseSqlite("Data Source=games.db"));

        // 2️⃣ usługi domenowe
        sc.AddScoped<IAuthService, AuthService>();
        sc.AddScoped<IGameRepository, GameRepositorySqlite>();
        sc.AddScoped<IProfileService, ProfileService>();
        sc.AddScoped<IFriendService, FriendService>();

        // 3️⃣ walidatory
        sc.AddScoped<GameUploadValidator>();
        sc.AddScoped<RegisterValidator>();
        sc.AddScoped<UserValidator>();

        // 4️⃣ view-modele  (Transient → każdy widok dostaje „świeży” VM)
        sc.AddTransient<LoginViewModel>();
        sc.AddTransient<MainViewModel>();
        sc.AddTransient<UsersViewModel>();
        sc.AddTransient<ProfileViewModel>();
        sc.AddTransient<FriendListViewModel>();
        sc.AddTransient<AddGameDialogViewModel>();
        sc.AddTransient<ShellViewModel>();

        Services = sc.BuildServiceProvider();

        /* ========== 2.  Migracja + seed admina ========== */
        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();

            if (!db.Users.Any())
            {
                var hasher = new PasswordHasher<User>();
                db.Users.Add(new User
                {
                    Login = "admin",
                    Email = "admin@example.com",
                    Role = UserRole.Admin,
                    PasswordHash = hasher.HashPassword(null!, "admin")
                });
                db.SaveChanges();
            }
        }

        /* ========== 3.  Okno logowania (modalne) ========== */
        var loginWin = new LoginWindow
        {
            DataContext = Services.GetRequiredService<LoginViewModel>()
        };
        loginWin.ShowDialog();      //  czeka, aż użytkownik się uwierzytelni

        var auth = Services.GetRequiredService<IAuthService>();
        if (auth.CurrentRole == UserRole.Guest)
        {
            //  anulowano / zamknięto okno logowania
            Shutdown();
            return;
        }

        /* ========== 4.  Shell z nawigacją ========== */
        var shell = new ShellWindow(
                        Services.GetRequiredService<ShellViewModel>())
        {
            Title = auth.CurrentRole == UserRole.Admin
                    ? "GamePlatform – Administrator"
                    : "GamePlatform"
        };

        MainWindow = shell;
        shell.Show();
    }
}
