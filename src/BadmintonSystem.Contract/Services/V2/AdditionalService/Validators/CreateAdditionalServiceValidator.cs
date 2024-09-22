using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.AdditionalService.Validators;
public class CreateAdditionalServiceValidator : AbstractValidator<Command.CreateAdditionalServiceCommand>
{
    public CreateAdditionalServiceValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null")
                                 .Length(2, 20).WithMessage("Valid name must be bettwen 2 to 20 charaters.");

        RuleFor(x => x.Data.Price).GreaterThan(0).WithMessage("Price must greater than 0");
    }
}
