using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Shared.DTO;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Validation;
using LiveChartsCore.Geo;
using Microsoft.Win32;
using System.Net.Sockets;

namespace GamePlatform.Desktop.ViewModels;

public partial class AddGameDialogViewModel(GameUploadValidator validator) : ObservableObject
{
    [ObservableProperty] public string title = string.Empty;
    [ObservableProperty] public string version = "1.0.0";
    [ObservableProperty] public string zipPath = string.Empty;
    [ObservableProperty] public GameCategory selectedCategory = GameCategory.Other;
    [ObservableProperty] public GameMode selectedMode = GameMode.SinglePlayer;
    [ObservableProperty] public int? maxPlayers;
    [ObservableProperty] public bool isLive;
    [ObservableProperty] public string? description;
    [ObservableProperty] public string? error;

    [RelayCommand]
    private void PickZip()
    {
        var dlg = new OpenFileDialog { Filter = "ZIP (*.zip)|*.zip" };
        if (dlg.ShowDialog() == true) ZipPath = dlg.FileName;
    }

    public bool Validate()
    {
        var dto = new GameUploadDto(Title, Version, ZipPath, SelectedCategory, SelectedMode, MaxPlayers, IsLive, Description);
        var res = validator.Validate(dto);
        if (!res.IsValid) { Error = res.ToString("• "); return false; }
        return true;
    }
}