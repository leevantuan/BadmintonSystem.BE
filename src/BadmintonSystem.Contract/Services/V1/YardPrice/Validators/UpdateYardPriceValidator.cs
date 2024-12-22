using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.YardPrice.Validators;

public sealed class UpdateYardPriceValidator : AbstractValidator<Command.UpdateYardPriceCommand>
{
    public UpdateYardPriceValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");
    }
}
