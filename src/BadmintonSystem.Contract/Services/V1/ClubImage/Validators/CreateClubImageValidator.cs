using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.ClubImage.Validators;

public sealed class CreateClubImageValidator : AbstractValidator<Command.CreateClubImageCommand>
{
    public CreateClubImageValidator()
    {
        RuleFor(x => x.Data.ImageLink).NotEmpty().WithMessage("ImageLink not null or empty");

        RuleFor(x => x.Data.ClubId).NotEmpty().WithMessage("ClubId not null or empty");
    }
}
