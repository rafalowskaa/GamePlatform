using FluentValidation;
using GamePlatform.Shared.Validation;

namespace GamePlatform.Shared.Validation;

public record RegisterDto(string Login, string Email, string Password, string PasswordRepeat);

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Login).NotEmpty().Length(3, 30);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.PasswordRepeat)
            .Equal(dto => dto.Password)
            .WithMessage("Hasła nie są zgodne");
    }
}
