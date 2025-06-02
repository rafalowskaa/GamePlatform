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

    [ObservableProperty] private string _titlePl = "";
    [ObservableProperty] private string _titleEn = "";
    [ObservableProperty] private string _version = "1.0.0";
    [ObservableProperty] private string _zipPath = "";
    private string? _coverPath;

    /// <summary>
    /// Ścieżka do wybranego obrazka okładki.
    /// Dodajemy jawne właściwości, aby projekt kompilował się również bez
    /// uruchomionych generatorów źródeł.
    /// </summary>
    public string? CoverPath
    {
        get => _coverPath;
        set => SetProperty(ref _coverPath, value);
    }
    [ObservableProperty] private GameCategory _selectedCategory = GameCategory.Other;
    [ObservableProperty] private GameMode _selectedMode = GameMode.SinglePlayer;
    [ObservableProperty] private int? _maxPlayers;
    [ObservableProperty] private bool _isLive;
    [ObservableProperty] private string? _descriptionPl;
    [ObservableProperty] private string? _descriptionEn;
    [ObservableProperty] private string? _error;

    [RelayCommand]
    private void PickZip()
    {
        var dlg = new OpenFileDialog { Filter = "Game package (*.zip;*.exe)|*.zip;*.exe" };
        if (dlg.ShowDialog() == true) ZipPath = dlg.FileName;
    }

    [RelayCommand]
    private void PickCover()
    {
        var dlg = new OpenFileDialog { Filter = "Image (*.jpg;*.png)|*.jpg;*.png" };
        if (dlg.ShowDialog() == true) CoverPath = dlg.FileName;
    }

    public bool Validate()
    {
        var dto = new GameUploadDto(TitlePl, TitleEn, Version, ZipPath, SelectedCategory,
                                    SelectedMode, MaxPlayers, IsLive, DescriptionPl, DescriptionEn, CoverPath);
        var res = _validator.Validate(dto);
        if (!res.IsValid) { Error = res.ToString("• "); return false; }
        return true;
    }
}
