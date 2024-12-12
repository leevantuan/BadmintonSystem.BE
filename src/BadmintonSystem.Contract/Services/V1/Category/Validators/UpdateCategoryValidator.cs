using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Category.Validators;
public sealed class UpdateCategoryValidator :
    AbstractValidator<Command.UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");

        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null")
            .MaximumLength(200).WithMessage("Name maximum 200 character");
    }
}
