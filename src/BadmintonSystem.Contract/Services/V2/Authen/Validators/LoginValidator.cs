using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.Authen.Validators;
public class LoginValidator : AbstractValidator<Query.Login>
{
    public LoginValidator()
    {
        // Rule của nghiệp vụ
        // Check validator cho create
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email not null")
                             .EmailAddress().WithMessage("Email valid!");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password not null")
                    .Length(2, 20).WithMessage("Valid Password must be bettwen 2 to 20 charaters.");
        //.Length(2, 20).WithMessage("Valid name must be bettwen 2 to 20 charaters."); // không null
        //RuleFor(x => x.Price).GreaterThan(0); // phải lớn hơn 0
        //RuleFor(x => x.Description).NotEmpty(); // không null
    }
}
