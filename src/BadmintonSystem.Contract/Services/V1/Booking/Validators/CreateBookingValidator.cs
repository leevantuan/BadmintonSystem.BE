using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Booking.Validators;

public sealed class CreateBookingValidator : AbstractValidator<Command.CreateBookingCommand>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.Data.BookingDate).NotEmpty().WithMessage("BookingDate not null or empty");

        RuleFor(x => x.Data.BookingTotal).NotEmpty().WithMessage("BookingTotal not null or empty");

        RuleFor(x => x.Data.BookingStatus).NotEmpty().WithMessage("BookingStatus not null or empty");

        RuleFor(x => x.Data.PaymentStatus).NotEmpty().WithMessage("PaymentStatus not null or empty");

        RuleFor(x => x.Data.UserId).NotEmpty().WithMessage("UserId not null or empty");

        RuleFor(x => x.Data.SaleId).NotEmpty().WithMessage("SaleId not null or empty");
    }
}
