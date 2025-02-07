using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.User.Validators;

public sealed class RegisterByCustomerValidator : AbstractValidator<Query.RegisterByCustomerQuery>
{
    public RegisterByCustomerValidator()
    {
        RuleFor(x => x.Data.UserName).NotEmpty().WithMessage("UserName not null or empty");
        RuleFor(x => x.Data.Email).EmailAddress().WithMessage("Email not null or empty");
        RuleFor(x => x.Data.Password).NotEmpty().WithMessage("Password not null or empty");
        RuleFor(x => x.Data.PhoneNumber).NotEmpty().WithMessage("PhoneNumber not null or empty");
        RuleFor(x => x.Data.Gender).NotEmpty().WithMessage("Gender not null or empty");
    }
}
