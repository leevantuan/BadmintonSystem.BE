using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Yard.Validators;

public sealed class DeleteMultipleYardValidator : AbstractValidator<Command.DeleteYardsCommand>
{
    public DeleteMultipleYardValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
