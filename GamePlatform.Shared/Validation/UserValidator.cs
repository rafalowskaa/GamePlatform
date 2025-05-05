using FluentValidation;
using GamePlatform.Shared.Models;

namespace GamePlatform.Shared.Validation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Login)
            .NotEmpty().Length(3, 50);

        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(u => u.PasswordHash)
            .NotEmpty();                       // hash zawsze ustawiany

        RuleFor(u => u.Role)
            .IsInEnum();
    }
}
