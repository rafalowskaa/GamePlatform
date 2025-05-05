using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamePlatform.Shared.DTO;
using GamePlatform.Shared.Enums;
using GamePlatform.Shared.Validation;
using Microsoft.Win32;

namespace GamePlatform.Desktop.ViewModels
{
    public partial class AddGameDialogViewModel : ObservableObject
    {
        private readonly GameUploadValidator _validator;

        public AddGameDialogViewModel(GameUploadValidator validator) => _validator = validator;

        private string _title = string.Empty;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        private string _version = "1.0.0";
        public string Version { get => _version; set => SetProperty(ref _version, value); }

        private string _zipPath = string.Empty;
        public string ZipPath { get => _zipPath; set => SetProperty(ref _zipPath, value); }

        private GameCategory _selectedCategory = GameCategory.Other;
        public GameCategory SelectedCategory { get => _selectedCategory; set => SetProperty(ref _selectedCategory, value); }

        private GameMode _selectedMode = GameMode.SinglePlayer;
        public GameMode SelectedMode { get => _selectedMode; set => SetProperty(ref _selectedMode, value); }

        private int? _maxPlayers;
        public int? MaxPlayers { get => _maxPlayers; set => SetProperty(ref _maxPlayers, value); }

        private bool _isLive;
        public bool IsLive { get => _isLive; set => SetProperty(ref _isLive, value); }

        private string? _description;
        public string? Description { get => _description; set => SetProperty(ref _description, value); }

        private string? _error;
        public string? Error { get => _error; set => SetProperty(ref _error, value); }

        [RelayCommand]
        private void PickZip()
        {
            var dlg = new OpenFileDialog { Filter = "ZIP (*.zip)|*.zip" };
            if (dlg.ShowDialog() == true)
                ZipPath = dlg.FileName;
        }

        public bool Validate()
        {
            var dto = new GameUploadDto(Title, Version, ZipPath, SelectedCategory, SelectedMode, MaxPlayers, IsLive, Description);
            var res = _validator.Validate(dto);

            if (!res.IsValid)
            {
                Error = res.ToString("• ");
                return false;
            }
            return true;
        }
    }
}
