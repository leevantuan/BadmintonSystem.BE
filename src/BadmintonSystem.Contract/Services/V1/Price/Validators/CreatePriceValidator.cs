using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Price.Validators;

public sealed class CreatePriceValidator : AbstractValidator<Command.CreatePriceCommand>
{
    public CreatePriceValidator()
    {
        RuleFor(x => x.Data.YardPrice).NotEmpty().WithMessage("Yard Price not null");

        RuleFor(x => x.Data.IsDefault).NotEmpty().WithMessage("Is Default not null");
    }
}
