using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.AdditionalService.Validators;
public class UpdateAdditionalServiceValidator : AbstractValidator<Command.UpdateAdditionalServiceCommand>
{
    public UpdateAdditionalServiceValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Valid Id!");

        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Valid name")
                            .Length(2, 50).WithMessage("Name must between 2 to 50 characters.");

        RuleFor(x => x.Data.Price).GreaterThan(0).WithMessage("Price must greater than 0");
    }
}
