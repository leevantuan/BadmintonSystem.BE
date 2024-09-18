using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.Category.Validators;
public class UpdateCategoryValidator : AbstractValidator<Command.UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Valid Id!");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Valid name")
                            .Length(2, 50).WithMessage("Name must between 2 to 50 characters."); // không null
    }
}
