// App.xaml.cs  (wymień cały plik)

// ▸ using-i bez zmian
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

public partial class App : Application
{
    public IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        // 0️⃣  Wyłączamy automatyczne zamykanie – przywrócimy je później
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        base.OnStartup(e);

        // 1️⃣  DI
        var sc = new ServiceCollection();

        sc.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=games.db"));

        sc.AddScoped<IAuthService, AuthService>();
        sc.AddScoped<IGameRepository, GameRepositorySqlite>();
        sc.AddScoped<GameUploadValidator>();
        sc.AddScoped<RegisterValidator>();
        sc.AddScoped<UserValidator>();

        sc.AddTransient<LoginViewModel>();
        sc.AddTransient<MainViewModel>();
        sc.AddTransient<AdminViewModel>();
        sc.AddTransient<AddGameDialogViewModel>();

        Services = sc.BuildServiceProvider();

        // 2️⃣  Migracje + seed admina
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

        // 3️⃣  Okno logowania
        var loginVm = Services.GetRequiredService<LoginViewModel>();
        var loginWin = new LoginWindow { DataContext = loginVm };
        loginWin.ShowDialog();                 // czekamy na zamknięcie

        var auth = Services.GetRequiredService<IAuthService>();
        if (auth.CurrentRole == UserRole.Guest)
        {
            // ręczne zamknięcie aplikacji
            Shutdown();
            return;
        }

        // 4️⃣  Shell zależny od roli
        Window shell = auth.CurrentRole switch
        {
            UserRole.Admin => new AdminWindow { DataContext = Services.GetRequiredService<AdminViewModel>() },
            _ => new MainWindow { DataContext = Services.GetRequiredService<MainViewModel>() }
        };

        MainWindow = shell;

        // PRZYWRACAMY normalny tryb zamykania
        ShutdownMode = ShutdownMode.OnMainWindowClose;

        shell.Show();
    }
}
