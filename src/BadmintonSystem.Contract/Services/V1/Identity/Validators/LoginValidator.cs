using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Identity.Validators;
public sealed class LoginValidator : AbstractValidator<Query.LoginQuery>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
