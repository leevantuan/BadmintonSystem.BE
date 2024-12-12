using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.PaymentMethod.Validators;

public sealed class CreatePaymentMethodValidator : AbstractValidator<Command.CreatePaymentMethodCommand>
{
    public CreatePaymentMethodValidator()
    {
        RuleFor(x => x.Data.AccountNumber).NotEmpty().WithMessage("Account number not null or empty");

        RuleFor(x => x.Data.Expiry).NotEmpty().WithMessage("Expiry not null or empty");

        RuleFor(x => x.Data.Default).NotEmpty().WithMessage("Default not null or empty");

        RuleFor(x => x.Data.Provider).NotEmpty().WithMessage("Provider not null or empty");

        RuleFor(x => x.Data.UserId).NotEmpty().WithMessage("UserId not null or empty");
    }
}
