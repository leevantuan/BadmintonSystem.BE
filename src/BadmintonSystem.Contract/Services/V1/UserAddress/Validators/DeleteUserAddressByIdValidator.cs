using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.UserAddress.Validators;

public sealed class DeleteUserAddressByIdValidator : AbstractValidator<Command.DeleteUserAddressByIdCommand>
{
    public DeleteUserAddressByIdValidator()
    {
        RuleFor(x => x.Data.UserId).NotEmpty().WithMessage("UserId not null or empty");

        RuleFor(x => x.Data.AddressId).NotEmpty().WithMessage("AddressId not null or empty");
    }
}
