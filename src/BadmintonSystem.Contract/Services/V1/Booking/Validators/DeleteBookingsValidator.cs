using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Booking.Validators;

public sealed class DeleteBookingsValidator : AbstractValidator<Command.DeleteBookingsCommand>
{
    public DeleteBookingsValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null or empty");
    }
}
