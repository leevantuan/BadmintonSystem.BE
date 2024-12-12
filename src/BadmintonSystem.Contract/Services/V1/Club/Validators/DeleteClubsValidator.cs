using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Club.Validators;

public sealed class DeleteClubsValidator : AbstractValidator<Command.DeleteClubsCommand>
{
    public DeleteClubsValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null or empty");
    }
}
