using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Sale.Validators;

public sealed class DeleteSalesValidator : AbstractValidator<Command.DeleteSalesCommand>
{
    public DeleteSalesValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null or empty");
    }
}
