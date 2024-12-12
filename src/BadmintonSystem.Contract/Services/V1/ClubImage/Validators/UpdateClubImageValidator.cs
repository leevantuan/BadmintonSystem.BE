using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.ClubImage.Validators;

public sealed class UpdateClubImageValidator : AbstractValidator<Command.UpdateClubImageCommand>
{
    public UpdateClubImageValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null or empty");

        RuleFor(x => x.Data.ImageLink).NotEmpty().WithMessage("ImageLink not null or empty");

        RuleFor(x => x.Data.ClubId).NotEmpty().WithMessage("ClubId not null or empty");
    }
}
