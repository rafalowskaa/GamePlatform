using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using GamePlatform.Shared.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public partial class ReportsViewModel : ObservableObject
{
    private readonly IReportService _svc;
    public ReportsViewModel(IReportService svc) { _svc = svc; _ = RefreshAsync(); }

    public ObservableCollection<UserReport> Reports { get; } = new();

    [RelayCommand]
    private async Task RefreshAsync()
    {
        Reports.Clear();
        foreach (var r in await _svc.GetAllAsync()) Reports.Add(r);
    }
}
