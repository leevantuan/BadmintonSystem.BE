using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.BookingLine.Validators;

public sealed class DeleteBookingLinesValidator : AbstractValidator<Command.DeleteBookingLinesCommand>
{
    public DeleteBookingLinesValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null or empty");
    }
}
