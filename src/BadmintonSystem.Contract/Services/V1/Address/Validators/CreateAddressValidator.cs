using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Address.Validators;

public sealed class CreateAddressValidator : AbstractValidator<Command.CreateAddressCommand>
{
    public CreateAddressValidator()
    {
        RuleFor(x => x.Data.Unit).NotEmpty().WithMessage("Unit not null or empty");

        RuleFor(x => x.Data.Street).NotEmpty().WithMessage("Street not null or empty");

        RuleFor(x => x.Data.AddressLine1).NotEmpty().WithMessage("AddressLine1 not null or empty");

        RuleFor(x => x.Data.AddressLine2).NotEmpty().WithMessage("AddressLine2 not null or empty");

        RuleFor(x => x.Data.City).NotEmpty().WithMessage("City not null or empty");
    }
}
