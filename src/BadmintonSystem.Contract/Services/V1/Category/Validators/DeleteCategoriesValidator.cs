using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Category.Validators;

public sealed class DeleteCategoriesValidator :
    AbstractValidator<Command.DeleteCategoriesCommand>
{
    public DeleteCategoriesValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
