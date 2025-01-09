using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.YardType.Validators;

public sealed class CreateYardTypeValidator : AbstractValidator<Command.CreateYardTypeCommand>
{
    public CreateYardTypeValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null");
    }
}
