using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.Sale.Validators;
public class UpdateSaleValidator : AbstractValidator<Command.UpdateSaleCommand>
{
    public UpdateSaleValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Valid Id!");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Valid name")
                            .Length(2, 50).WithMessage("Name must between 2 to 50 characters."); // không null

        RuleFor(x => x.Persent).NotEmpty().WithMessage("Not Null")
                                .GreaterThan(0).WithMessage("Persent must greater than 0");
    }
}
