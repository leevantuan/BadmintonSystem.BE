using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Address.Validators;

public sealed class DeleteAddressesValidator : AbstractValidator<Command.DeleteAddressesCommand>
{
    public DeleteAddressesValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null or empty");
    }
}
