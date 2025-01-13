using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Booking.Validators;

public sealed class DeleteBookingsValidator : AbstractValidator<Command.DeleteBookingByIdCommand>
{
    public DeleteBookingsValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Ids not null or empty");
    }
}
