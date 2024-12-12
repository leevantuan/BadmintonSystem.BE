using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Yard.Validators;

public sealed class CreateYardValidator : AbstractValidator<Command.CreateYardCommand>
{
    public CreateYardValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null");

        RuleFor(x => x.Data.YardTypeId).NotEmpty().WithMessage("Yard Id not null");
    }
}
