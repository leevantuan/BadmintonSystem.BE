using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.BookingTime.Validators;

public sealed class CreateBookingTimeValidator : AbstractValidator<Command.CreateBookingTimeCommand>
{
    public CreateBookingTimeValidator()
    {
        RuleFor(x => x.Data.TimeSlotId).NotEmpty().WithMessage("TimeSlotId not null or empty");

        RuleFor(x => x.Data.BookingLineId).NotEmpty().WithMessage("BookingLineId not null or empty");
    }
}
