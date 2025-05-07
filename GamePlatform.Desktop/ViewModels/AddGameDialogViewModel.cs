using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Shared.DTO;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Validation;
using Microsoft.Win32;

namespace GamePlatform.Desktop.ViewModels;

public partial class AddGameDialogViewModel : ObservableObject
{
    private readonly GameUploadValidator _validator;
    public AddGameDialogViewModel(GameUploadValidator validator) => _validator = validator;

    [ObservableProperty] private string _title = "";
    [ObservableProperty] private string _version = "1.0.0";
    [ObservableProperty] private string _zipPath = "";
    [ObservableProperty] private GameCategory _selectedCategory = GameCategory.Other;
    [ObservableProperty] private GameMode _selectedMode = GameMode.SinglePlayer;
    [ObservableProperty] private int? _maxPlayers;
    [ObservableProperty] private bool _isLive;
    [ObservableProperty] private string? _description;
    [ObservableProperty] private string? _error;

    [RelayCommand]
    private void PickZip()
    {
        var dlg = new OpenFileDialog { Filter = "ZIP (*.zip)|*.zip" };
        if (dlg.ShowDialog() == true) ZipPath = dlg.FileName;
    }

    public bool Validate()
    {
        var dto = new GameUploadDto(Title, Version, ZipPath, SelectedCategory,
                                    SelectedMode, MaxPlayers, IsLive, Description);
        var res = _validator.Validate(dto);
        if (!res.IsValid) { Error = res.ToString("• "); return false; }
        return true;
    }
}
