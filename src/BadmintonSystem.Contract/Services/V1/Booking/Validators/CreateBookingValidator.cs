using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Booking.Validators;

public sealed class CreateBookingValidator : AbstractValidator<Command.CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.Data.YardPriceIds).NotEmpty().WithMessage("YardPriceIds not null or empty");
    }
}
