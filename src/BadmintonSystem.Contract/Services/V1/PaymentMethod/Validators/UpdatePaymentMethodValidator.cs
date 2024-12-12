using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.PaymentMethod.Validators;

public sealed class UpdatePaymentMethodValidator : AbstractValidator<Command.UpdatePaymentMethodCommand>
{
    public UpdatePaymentMethodValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null or empty");

        // RuleFor(x => x.Data.AccountNumber).NotEmpty().WithMessage("Account number not null or empty");
        //
        // RuleFor(x => x.Data.Expiry).NotEmpty().WithMessage("Expiry not null or empty");
        //
        // RuleFor(x => x.Data.Default).NotEmpty().WithMessage("Default not null or empty");
        //
        // RuleFor(x => x.Data.Provider).NotEmpty().WithMessage("Provider not null or empty");
        //
        // RuleFor(x => x.Data.UserId).NotEmpty().WithMessage("UserId not null or empty");
    }
}
