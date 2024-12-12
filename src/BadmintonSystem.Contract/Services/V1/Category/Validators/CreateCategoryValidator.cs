using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Category.Validators;
public sealed class CreateCategoryValidator :
    AbstractValidator<Command.CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null")
            .MaximumLength(200).WithMessage("Name maximum 200 character");
    }
}
