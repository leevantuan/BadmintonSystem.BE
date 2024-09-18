using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.Category.Validators;
public class DeleteCategoryValidator : AbstractValidator<Command.DeleteCategoryCommand>
{
    public DeleteCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty(); // không null
    }
}
