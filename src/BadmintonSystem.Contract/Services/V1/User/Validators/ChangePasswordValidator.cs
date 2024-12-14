using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.User.Validators;

public sealed class ChangePasswordValidator : AbstractValidator<Command.ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null or empty");

        RuleFor(x => x.Data.ConfirmPassword).NotEmpty().WithMessage("Confirm Password not null or empty");

        RuleFor(x => x.Data.Password).NotEmpty().WithMessage("Password not null or empty");
    }
}
