using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.User.Validators;

public sealed class ForgetPasswordValidator : AbstractValidator<Command.ForgetPasswordCommand>
{
    public ForgetPasswordValidator()
    {
        RuleFor(x => x.Data.Email).EmailAddress().WithMessage("Email not null or empty");

        RuleFor(x => x.Data.ConfirmPassword).NotEmpty().WithMessage("Confirm Password not null or empty");

        RuleFor(x => x.Data.Password).NotEmpty().WithMessage("Password not null or empty");
    }
}
