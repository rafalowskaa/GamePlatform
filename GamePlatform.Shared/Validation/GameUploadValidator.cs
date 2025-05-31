using FluentValidation;
using GamePlatform.Shared.DTO;
using System.IO;      //  <-- dodane

namespace GamePlatform.Shared.Validation
{
    public class GameUploadValidator : AbstractValidator<GameUploadDto>
    {
        public GameUploadValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Version).NotEmpty();

            RuleFor(x => x.ZipPath)
                .Must(File.Exists)
                .WithMessage("Plik nie istnieje")
                .Must(p =>
                {
                    var ext = Path.GetExtension(p).ToLowerInvariant();
                    return ext is ".zip" or ".exe";
                })
                .WithMessage("Obsługiwane są tylko pliki .zip lub .exe");

            RuleFor(x => x.MaxPlayers)
                .Must(p => p is null || p > 0)
                .WithMessage("Max graczy musi być dodatnie");
        }
    }
}
