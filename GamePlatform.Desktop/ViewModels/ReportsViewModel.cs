using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Shapes;

public partial class ReportsViewModel : ObservableObject
{
    private readonly IReportService _svc;
    private readonly INotificationService _notify;

    public ReportsViewModel(IReportService svc, INotificationService notify)
    {
        _svc = svc;
        _notify = notify;
        _ = RefreshAsync();
    }

    public ObservableCollection<UserReport> Reports { get; } = new();

    [RelayCommand]
    private async Task RefreshAsync()
    {
        Reports.Clear();
        foreach (var r in await _svc.GetAllAsync()) Reports.Add(r);
    }

    [RelayCommand]
    private async Task WarnAsync(UserReport? r)
    {
        if (r is null) return;
        await _svc.SendWarningAsync(r.Id);
        r.Status = Shared.Enums.ReportStatus.Warned;
        _notify.Show($"Użytkownik {r.ReportedUser.Login} został ostrzeżony.");
    }

    [RelayCommand]
    private async Task CancelAsync(UserReport? r)
    {
        if (r is null) return;
        await _svc.CancelAsync(r.Id);
        await RefreshAsync();
    }

    [RelayCommand]
    private async Task BlockUserAsync(UserReport? r)
    {
        if (r is null) return;
        await _svc.BlockReportedUserAsync(r.Id);
        await RefreshAsync();
    }

    [RelayCommand]
    private async Task DeleteAsync(UserReport? r)
    {
        if (r is null) return;
        await _svc.DeleteAsync(r.Id);
        await RefreshAsync();
    }
}
