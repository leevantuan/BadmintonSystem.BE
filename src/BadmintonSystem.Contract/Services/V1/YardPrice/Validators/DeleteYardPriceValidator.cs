using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.YardPrice.Validators;

public sealed class DeleteYardPriceValidator : AbstractValidator<Command.DeleteYardPricesCommand>
{
    public DeleteYardPriceValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
