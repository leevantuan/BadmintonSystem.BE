using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Club.Validators;

public sealed class CreateClubValidator : AbstractValidator<Command.CreateClubCommand>
{
    public CreateClubValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null or empty");

        RuleFor(x => x.Data.Hotline).NotEmpty().WithMessage("Hotline not null or empty");

        RuleFor(x => x.Data.OpeningTime).NotEmpty().WithMessage("OpeningTime not null or empty");

        RuleFor(x => x.Data.ClosingTime).NotEmpty().WithMessage("ClosingTime not null or empty");

        RuleFor(x => x.Data.Code).NotEmpty().WithMessage("Code not null or empty");
    }
}
