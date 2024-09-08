using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.Gender.Validators;
public class DeleteGenderValidator : AbstractValidator<Command.DeleteGenderCommand>
{
    public DeleteGenderValidator()
    {
        // Rule của nghiệp vụ
        // Check validator cho Delete
        RuleFor(x => x.Id).NotEmpty(); // không null
        //RuleFor(x => x.Price).GreaterThan(0); // phải lớn hơn 0
        //RuleFor(x => x.Description).NotEmpty(); // không null
    }
}
