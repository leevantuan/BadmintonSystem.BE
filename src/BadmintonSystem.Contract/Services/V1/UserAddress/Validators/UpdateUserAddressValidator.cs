using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.UserAddress.Validators;

public sealed class UpdateUserAddressValidator : AbstractValidator<Command.UpdateUserAddressCommand>
{
    public UpdateUserAddressValidator()
    {
        RuleFor(x => x.Data.AddressId).NotEmpty().WithMessage("AddressId not null or empty");

        RuleFor(x => x.Data.UserId).NotEmpty().WithMessage("UserId not null or empty");

        RuleFor(x => x.Data.IsDefault).NotEmpty().WithMessage("IsDefault not null or empty");
    }
}
