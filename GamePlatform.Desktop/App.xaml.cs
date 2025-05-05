using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Services;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.ViewModels;
using GamePlatform.Shared.Validation;

namespace GamePlatform.Desktop;

public partial class App : Application
{
    public IServiceProvider Services { get; }

    public App()
    {
        var sc = new ServiceCollection();
        sc.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=games.db"));
        sc.AddSingleton<IGameRepository, GameRepositorySqlite>();
        sc.AddTransient<GameUploadValidator>();
        sc.AddTransient<AddGameDialogViewModel>();
        sc.AddTransient<MainViewModel>();
        Services = sc.BuildServiceProvider();

        using (var scope = Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();          // <-- włącza wszystkie migracje
        }
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var vm = Services.GetRequiredService<MainViewModel>();
        await vm.LoadAsync();
        new MainWindow { DataContext = vm }.Show();
    }
}