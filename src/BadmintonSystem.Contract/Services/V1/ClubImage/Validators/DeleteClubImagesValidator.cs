using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.ClubImage.Validators;

public sealed class DeleteClubImagesValidator : AbstractValidator<Command.DeleteClubImagesCommand>
{
    public DeleteClubImagesValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null or empty");
    }
}
