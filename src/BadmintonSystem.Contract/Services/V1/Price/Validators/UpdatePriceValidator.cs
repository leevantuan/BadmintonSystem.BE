using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Price.Validators;

public sealed class UpdatePriceValidator : AbstractValidator<Command.UpdatePriceCommand>
{
    public UpdatePriceValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");
    }
}
