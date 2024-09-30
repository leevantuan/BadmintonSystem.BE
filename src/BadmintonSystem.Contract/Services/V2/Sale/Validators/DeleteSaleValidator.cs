using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.Sale.Validators;
public class DeleteSaleValidator : AbstractValidator<Command.DeleteSaleCommand>
{
    public DeleteSaleValidator()
    {
        RuleFor(x => x.Id).NotEmpty(); // không null
    }
}
