using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamePlatform.Desktop.ViewModels;

public partial class ReportDialogViewModel : ObservableObject
{
    private readonly IReportService _svc;
    private readonly Guid _meId;
    private readonly Guid _targetId;

    public ReportDialogViewModel(IReportService svc, Guid meId, Guid targetId)
    {
        _svc = svc;
        _meId = meId;
        _targetId = targetId;

        Categories = new[] { "Spam", "Obraźliwe treści", "Cheat / Hack", "Inne" };
        SelectedCategory = Categories[0];
    }

    public IReadOnlyList<string> Categories { get; }

    [ObservableProperty] private string selectedCategory;
    [ObservableProperty] private string description = "";

    [RelayCommand]
    private async Task Submit(System.Windows.Window win)
    {
        if (string.IsNullOrWhiteSpace(Description))
            return;

        await _svc.SubmitAsync(_meId, _targetId, SelectedCategory, Description.Trim());
        win.DialogResult = true;
    }
}
