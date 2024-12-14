using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.User.Validators;

public sealed class RegisterByCustomerValidator : AbstractValidator<Query.RegisterByCustomerQuery>
{
    public RegisterByCustomerValidator()
    {
        RuleFor(x => x.Data.UserName).NotEmpty().WithMessage("UserName not null or empty");
        RuleFor(x => x.Data.Email).EmailAddress().WithMessage("Email not null or empty");
        RuleFor(x => x.Data.FirstName).NotEmpty().WithMessage("FirstName not null or empty");
        RuleFor(x => x.Data.LastName).NotEmpty().WithMessage("LastName not null or empty");
        RuleFor(x => x.Data.Password).NotEmpty().WithMessage("Password not null or empty");
        RuleFor(x => x.Data.PhoneNumber).NotEmpty().WithMessage("PhoneNumber not null or empty");
        RuleFor(x => x.Data.Gender).NotEmpty().WithMessage("Gender not null or empty");
        RuleFor(x => x.Data.DateOfBirth).NotEmpty().WithMessage("DateOfBirth not null or empty");
        RuleFor(x => x.Data.AvatarUrl).NotEmpty().WithMessage("AvatarUrl not null or empty");
        RuleFor(x => x.Data.Unit).NotEmpty().WithMessage("Unit not null or empty");
        RuleFor(x => x.Data.Street).NotEmpty().WithMessage("Street not null or empty");
        RuleFor(x => x.Data.AddressLine1).NotEmpty().WithMessage("AddressLine1 not null or empty");
        RuleFor(x => x.Data.AddressLine2).NotEmpty().WithMessage("AddressLine2 not null or empty");
        RuleFor(x => x.Data.City).NotEmpty().WithMessage("City not null or empty");
        RuleFor(x => x.Data.Province).NotEmpty().WithMessage("Province not null or empty");
    }
}
