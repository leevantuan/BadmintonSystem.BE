using FluentValidation;

namespace BadmintonSystem.Contract.Services.Gender.Validators;
public class CreateGenderValidator : AbstractValidator<Command.CreateGenderCommand>
{
    public CreateGenderValidator()
    {
        // Rule của nghiệp vụ
        // Check validator cho create
        RuleFor(x => x.request.Data["Name"]).NotEmpty(); // không null
        //RuleFor(x => x.Price).GreaterThan(0); // phải lớn hơn 0
        //RuleFor(x => x.Description).NotEmpty(); // không null
    }
}
