using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.Club.Validators;
public class UpdateClubValidator : AbstractValidator<Command.UpdateClubCommand>
{
    public UpdateClubValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Valid Id!");

        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null")
                                 .Length(2, 20).WithMessage("Valid name must be bettwen 2 to 20 charaters.");

        RuleFor(x => x.Data.HotLine).NotEmpty().WithMessage("Hotline not null")
                                    .Length(4, 20).WithMessage("Hotline must be bettween 4 to 20 charaters.");
    }
}
