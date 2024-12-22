using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.YardType.Validators;

public sealed class CreateYardTypeValidator : AbstractValidator<Command.CreateYardTypeCommand>
{
    public CreateYardTypeValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null");

        RuleFor(x => x.Data.PriceId).NotEmpty().WithMessage("Price Id not null");
    }
}
