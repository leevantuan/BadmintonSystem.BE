using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Service.Validators;

public sealed class UpdateServiceValidator :
    AbstractValidator<Command.UpdateServiceCommand>
{
    public UpdateServiceValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");
    }
}
