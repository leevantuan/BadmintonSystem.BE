using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Club.Validators;

public sealed class UpdateClubValidator : AbstractValidator<Command.UpdateClubCommand>
{
    public UpdateClubValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null or empty");

        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null or empty");

        RuleFor(x => x.Data.Hotline).NotEmpty().WithMessage("Hotline not null or empty");

        RuleFor(x => x.Data.OpeningTime).NotEmpty().WithMessage("OpeningTime not null or empty");

        RuleFor(x => x.Data.ClosingTime).NotEmpty().WithMessage("ClosingTime not null or empty");

        RuleFor(x => x.Data.Code).NotEmpty().WithMessage("Code not null or empty");
    }
}
