using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.ViewModels;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using GamePlatform.Shared.Validation;
using MaterialDesignThemes.Wpf;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GamePlatform.Desktop.Localization;

namespace GamePlatform.Desktop;

/// <summary>Punkt startowy aplikacji.</summary>
public partial class App : Application
{
    /// <summary>Ułatwia dostęp do silnie typowanej instancji aplikacji.</summary>
    public static new App Current => (App)Application.Current;
    public IServiceProvider Services { get; private set; } = null!;
    public SnackbarMessageQueue MsgQueue { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        /* globalny catcher */
        DispatcherUnhandledException += (_, a) =>
        {
            MessageBox.Show(a.Exception.ToString(), "Błąd krytyczny",
                            MessageBoxButton.OK, MessageBoxImage.Error);
            a.Handled = true;
        };

        Localization.Current = Lang.English;

        /* —— DI + EF —— */
        var sc = new ServiceCollection();

        sc.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=games.db"));

        sc.AddScoped<IAuthService, AuthService>();
        sc.AddScoped<IGameRepository, GameRepositorySqlite>();
        sc.AddScoped<IProfileService, ProfileService>();
        sc.AddScoped<IFriendService, FriendService>();
        sc.AddScoped<IReportService, ReportService>();
        sc.AddScoped<IChatService, ChatService>();
        sc.AddScoped<IRankService, RankService>();

        sc.AddSingleton<INotificationService, NotificationService>();

        /* walidatory */
        sc.AddScoped<GameUploadValidator>();
        sc.AddScoped<RegisterValidator>();
        sc.AddScoped<UserValidator>();

        /* view-modele */
        sc.AddTransient<LoginViewModel>();
        sc.AddTransient<ShellViewModel>();
        sc.AddTransient<MainViewModel>();
        sc.AddTransient<UsersViewModel>();
        sc.AddTransient<FriendListViewModel>();
        sc.AddTransient<FindFriendsViewModel>();
        sc.AddTransient<PendingRequestsViewModel>();
        sc.AddTransient<ReportsViewModel>();
        sc.AddTransient<ProfileViewModel>();
        sc.AddTransient<StatsViewModel>();
        sc.AddTransient<AddGameDialogViewModel>();
        sc.AddTransient<AdminReviewViewModel>();
        sc.AddTransient<EditUserViewModel>();

        Services = sc.BuildServiceProvider();

        /* powiadomienia ↔ Snackbar */
        if (Services.GetRequiredService<INotificationService>() is NotificationService ns)
            ns.Raised += m => Dispatcher.Invoke(() => MsgQueue.Enqueue(m));

        /* migracja + auto-admin */
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
                    PasswordHash = new PasswordHasher<User>().HashPassword(null!, "admin")
                });
                db.SaveChanges();
            }
        }

        MsgQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
        Resources["GlobalMsgQueue"] = MsgQueue;

        /* pętla logowania */
        for (; ; )
        {
            if (!ShowLogin()) { Shutdown(); return; }

            var shell = new ShellWindow(Services.GetRequiredService<ShellViewModel>());
            MainWindow = shell;
            shell.ShowDialog();

            // jeśli przelogowanie – kontynuuj pętlę
            if (Services.GetRequiredService<IAuthService>().CurrentRole != UserRole.Guest)
            {
                Shutdown();
                return;
            }
        }
    }

    /* —— helper —— */
    private bool ShowLogin()
    {
        var win = new LoginWindow
        {
            DataContext = Services.GetRequiredService<LoginViewModel>()
        };
        return win.ShowDialog() == true;
    }
}
