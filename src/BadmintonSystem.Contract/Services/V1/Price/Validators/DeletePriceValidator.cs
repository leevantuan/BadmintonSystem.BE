using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Price.Validators;

public sealed class DeletePriceValidator : AbstractValidator<Command.DeletePricesCommand>
{
    public DeletePriceValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
