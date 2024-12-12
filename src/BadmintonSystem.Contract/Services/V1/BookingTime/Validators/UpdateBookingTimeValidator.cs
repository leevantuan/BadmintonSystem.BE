using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.BookingTime.Validators;

public sealed class UpdateBookingTimeValidator : AbstractValidator<Command.UpdateBookingTimeCommand>
{
    public UpdateBookingTimeValidator()
    {
        RuleFor(x => x.Data.TimeSlotId).NotEmpty().WithMessage("TimeSlotId not null or empty");

        RuleFor(x => x.Data.BookingLineId).NotEmpty().WithMessage("BookingLineId not null or empty");
    }
}
