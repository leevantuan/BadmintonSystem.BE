using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.PaymentMethod.Validators;

public sealed class DeletePaymentMethodsValidator : AbstractValidator<Command.DeletePaymentMethodsCommand>
{
    public DeletePaymentMethodsValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null or empty");
    }
}
