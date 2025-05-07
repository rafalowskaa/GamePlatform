using FluentValidation;
using GamePlatform.Shared.Models;

namespace GamePlatform.Shared.Validation;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Login).NotEmpty().Length(3, 50);
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.RecoveryMail).EmailAddress().When(u => !string.IsNullOrWhiteSpace(u.RecoveryMail));
        RuleFor(u => u.PasswordHash).NotEmpty();
        RuleFor(u => u.Role).IsInEnum();
    }
}
