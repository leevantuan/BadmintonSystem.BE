using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Identity.Validators;

public sealed class ResetPasswordByIdValidator : AbstractValidator<Command.ResetPasswordByIdCommand>
{
    public ResetPasswordByIdValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null or empty");

        RuleFor(x => x.Data.Password).NotEmpty().WithMessage("Password not null or empty");
    }
}
