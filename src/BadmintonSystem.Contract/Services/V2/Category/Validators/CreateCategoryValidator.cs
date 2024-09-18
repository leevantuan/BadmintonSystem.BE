using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.Category.Validators;
public class CreateCategoryValidator : AbstractValidator<Command.CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null")
                                 .Length(2, 20).WithMessage("Valid name must be bettwen 2 to 20 charaters."); // không null
    }
}
