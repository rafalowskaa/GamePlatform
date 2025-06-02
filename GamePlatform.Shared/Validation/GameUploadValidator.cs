using FluentValidation;
using GamePlatform.Shared.DTO;
using System.IO;      //  <-- dodane

namespace GamePlatform.Shared.Validation
{
    public class GameUploadValidator : AbstractValidator<GameUploadDto>
    {
        public GameUploadValidator()
        {
            RuleFor(x => x.TitlePl).NotEmpty().MaximumLength(100);
            RuleFor(x => x.TitleEn).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Version).NotEmpty();

            RuleFor(x => x.ZipPath)
                .Must(File.Exists)
                .WithMessage("Plik nie istnieje")
                .Must(p =>
                {
                    var ext = Path.GetExtension(p).ToLowerInvariant();
                    return ext is ".zip" or ".exe";
                })
                .WithMessage("Obsługiwane są tylko pliki .zip lub .exe")
                .Must(p => new FileInfo(p).Length <= 300 * 1024 * 1024)
                .WithMessage("Maksymalny rozmiar to 300 MB");

            RuleFor(x => x.DescriptionPl).NotEmpty();
            RuleFor(x => x.DescriptionEn).NotEmpty();

            RuleFor(x => x.MaxPlayers)
                .Must(p => p is null || p > 0)
                .WithMessage("Max graczy musi być dodatnie");

            RuleFor(x => x.CoverPath)
                .Must(p => string.IsNullOrEmpty(p) || File.Exists(p))
                .WithMessage("Plik z obrazem nie istnieje");
        }
    }
}
