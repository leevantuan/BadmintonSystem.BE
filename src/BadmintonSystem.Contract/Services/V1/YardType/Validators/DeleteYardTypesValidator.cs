using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.YardType.Validators;

public sealed class DeleteMultipleYardTypeValidator : AbstractValidator<Command.DeleteYardTypesCommand>
{
    public DeleteMultipleYardTypeValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
