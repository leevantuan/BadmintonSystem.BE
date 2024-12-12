using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.ClubInformation.Validators;

public sealed class CreateClubInformationValidator : AbstractValidator<Command.CreateClubInformationCommand>
{
    public CreateClubInformationValidator()
    {
        RuleFor(x => x.Data.FacebookPageLink).NotEmpty().WithMessage("FacebookPageLink not null or empty");

        RuleFor(x => x.Data.InstagramLink).NotEmpty().WithMessage("InstagramLink not null or empty");

        RuleFor(x => x.Data.MapLink).NotEmpty().WithMessage("MapLink not null or empty");

        RuleFor(x => x.Data.ClubId).NotEmpty().WithMessage("ClubId not null or empty");
    }
}
