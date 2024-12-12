using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.ClubAddress.Validators;

public sealed class UpdateClubAddressValidator : AbstractValidator<Command.UpdateClubAddressCommand>
{
    public UpdateClubAddressValidator()
    {
        RuleFor(x => x.Data.AddressId).NotEmpty().WithMessage("AddressId not null or empty");

        RuleFor(x => x.Data.ClubId).NotEmpty().WithMessage("ClubId not null or empty");

        RuleFor(x => x.Data.Branch).NotEmpty().WithMessage("Branch not null or empty");
    }
}
