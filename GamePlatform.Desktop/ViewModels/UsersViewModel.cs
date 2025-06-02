using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Desktop.Views;
using GamePlatform.Shared.Data;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GamePlatform.Desktop.Localization;
using Loc = GamePlatform.Desktop.Localization.Localization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GamePlatform.Desktop.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private readonly IAuthService _auth;
    private readonly IServiceProvider _sp;

    public UsersViewModel(AppDbContext db, IAuthService auth, IServiceProvider sp)
    {
        _db = db;
        _auth = auth;
        _sp = sp;

        /* -------  lista opcji do ComboBoxa  ------- */
        Roles = new List<string>
        {
             Loc.TRaw("ALL"),
            nameof(UserRole.Admin),
            nameof(UserRole.Moderator),
            nameof(UserRole.User)
        };

        /* -------  CollectionView do filtrowania / sortowania  ------- */
        UsersView = CollectionViewSource.GetDefaultView(Users);
        UsersView.Filter = FilterPredicate;

        _ = LoadAsync();
    }

    /* --------------------  DANE  -------------------- */
    public ObservableCollection<User> Users { get; } = new();

    /// <summary>Widok z sortowaniem i filtrem.</summary>
    public ICollectionView UsersView { get; }

    /// <summary>Źródło ItemsSource dla ComboBoxa „Rola”.</summary>
    public IReadOnlyList<string> Roles { get; }

    /* --------------------  STAN UI  -------------------- */
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string? _selectedRoleFilter = Loc.TRaw("ALL");
    [ObservableProperty] private string? _loginFilter;
    [ObservableProperty] private bool _onlyBlocked;

    /* --------------------  ŁADOWANIE  -------------------- */
    [RelayCommand]
    private async Task LoadAsync()
    {
        IsBusy = true;

        Users.Clear();
        foreach (var u in await _db.Users.AsNoTracking().ToListAsync())
            Users.Add(u);

        IsBusy = false;
        UsersView.Refresh();
    }

    /* --------------------  FILTRACJA  -------------------- */
    private bool FilterPredicate(object obj)
    {
        if (obj is not User u) return false;

        /*  Rola  */
        if (!string.IsNullOrWhiteSpace(SelectedRoleFilter) &&
            SelectedRoleFilter != Loc.TRaw("ALL") &&
            !u.Role.ToString().Equals(SelectedRoleFilter, StringComparison.OrdinalIgnoreCase))
            return false;

        /*  Login  */
        if (!string.IsNullOrWhiteSpace(LoginFilter) &&
            !u.Login.Contains(LoginFilter, StringComparison.OrdinalIgnoreCase))
            return false;

        /*  Tylko z blokadą  */
        if (OnlyBlocked && string.IsNullOrWhiteSpace(u.BlockReason))
            return false;

        return true;
    }

    /*  automatyczne odświeżanie filtrów  */
    partial void OnSelectedRoleFilterChanged(string? _, string? __) => ApplyFilter();
    partial void OnLoginFilterChanged(string? _, string? __) => ApplyFilter();
    partial void OnOnlyBlockedChanged(bool _, bool __) => ApplyFilter();

    private void ApplyFilter()
    {
        UsersView.SortDescriptions.Clear();   // czyści ewentualne stare sortowanie
        UsersView.Refresh();
    }

    /*  wyczyść filtry  */
    [RelayCommand]
    private void ClearFilters()
    {
        SelectedRoleFilter = Loc.TRaw("ALL");
        LoginFilter = null;
        OnlyBlocked = false;

        UsersView.SortDescriptions.Clear();
        UsersView.Refresh();
    }

    /* --------------------  USUŃ  -------------------- */
    [RelayCommand]
    private async Task DeleteAsync(object? parameter)
    {
        if (parameter is not User victim) return;

        bool isModerator = _auth.CurrentRole == UserRole.Moderator;
        if (isModerator && victim.Role is UserRole.Admin or UserRole.Moderator) return;
        if (_auth.CurrentUser?.Id == victim.Id) return;

        if (MessageBox.Show(
                string.Format(Loc.TRaw("DELETE_USER_ASK"), victim.Login),
                Loc.TRaw("CONFIRM"), MessageBoxButton.YesNo, MessageBoxImage.Warning)
            != MessageBoxResult.Yes)
            return;

        /*  dialog z hasłem  */
        var pwdDlg = new PasswordPromptWindow { Owner = Application.Current.MainWindow };
        if (pwdDlg.ShowDialog() != true) return;

        var me = _auth.CurrentUser;
        if (me is null)
        {
            MessageBox.Show(Loc.TRaw("NO_USER_CONTEXT"),
                           Loc.TRaw("ERROR"), MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var hasher = new PasswordHasher<User>();
        if (hasher.VerifyHashedPassword(me, me.PasswordHash, pwdDlg.Password)
            == PasswordVerificationResult.Failed)
        {
            MessageBox.Show(Loc.TRaw("INVALID_PASSWORD"),
                           Loc.TRaw("ERROR"), MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        _db.Users.Remove(victim);
        await _db.SaveChangesAsync();
        Users.Remove(victim);
    }

    /* --------------------  EDYCJA  -------------------- */
    [RelayCommand]
    private async Task EditAsync(object? parameter)
    {
        if (parameter is not User victim) return;

        var dlgVm = _sp.GetRequiredService<EditUserViewModel>();
        dlgVm.Init(victim);

        var dlg = new EditUserDialog
        {
            DataContext = dlgVm,
            Owner = Application.Current.MainWindow
        };

        if (dlg.ShowDialog() == true)
            await LoadAsync();
    }
}
