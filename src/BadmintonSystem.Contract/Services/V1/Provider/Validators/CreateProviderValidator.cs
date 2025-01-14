using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Provider.Validators;

public sealed class CreateProviderValidator : AbstractValidator<Command.CreateProviderCommand>
{
    public CreateProviderValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null")
            .MaximumLength(200).WithMessage("Name maximum 200 character");

        RuleFor(x => x.Data.PhoneNumber).NotEmpty().WithMessage("PhoneNumber not null")
            .MaximumLength(20).WithMessage("Name maximum 20 character");

        RuleFor(x => x.Data.Address).NotEmpty().WithMessage("Address not null")
            .MaximumLength(200).WithMessage("Name maximum 200 character");
    }
}
