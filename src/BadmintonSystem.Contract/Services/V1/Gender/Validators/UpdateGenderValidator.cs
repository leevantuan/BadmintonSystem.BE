using BadmintonSystem.Contract.Services.V1.Gender;
using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Gender.Validators;
public class UpdateGenderValidator : AbstractValidator<Command.UpdateGenderCommand>
{
    public UpdateGenderValidator()
    {
        // Rule của nghiệp vụ
        // Check validator cho Update
        RuleFor(x => x.Id).NotEmpty().WithMessage("Valid Id!");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Valid name")
                            .Length(2, 50).WithMessage("Name must between 2 to 50 characters."); // không null
        //RuleFor(x => x.Price).GreaterThan(0); // phải lớn hơn 0
        //RuleFor(x => x.Description).NotEmpty(); // không null
    }
}
