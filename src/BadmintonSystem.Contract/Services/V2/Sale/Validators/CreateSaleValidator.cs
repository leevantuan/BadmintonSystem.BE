using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.Sale.Validators;
public class CreateSaleValidator : AbstractValidator<Command.CreateSaleCommand>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null")
                                 .Length(2, 20).WithMessage("Valid name must be bettwen 2 to 20 charaters."); // không null

        RuleFor(x => x.Data.Persent).NotEmpty().WithMessage("Not Null")
                        .GreaterThan(0).WithMessage("Persent must greater than 0");
    }
}
