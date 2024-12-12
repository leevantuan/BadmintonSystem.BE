using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Address.Validators;

public sealed class UpdateAddressValidator : AbstractValidator<Command.UpdateAddressCommand>
{
    public UpdateAddressValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null or empty");
    }
}
