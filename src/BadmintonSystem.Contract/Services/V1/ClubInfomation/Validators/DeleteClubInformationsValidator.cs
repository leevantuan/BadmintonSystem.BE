using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.ClubInformation.Validators;

public sealed class DeleteClubInformationsValidator : AbstractValidator<Command.DeleteClubInformationsCommand>
{
    public DeleteClubInformationsValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null or empty");
    }
}
