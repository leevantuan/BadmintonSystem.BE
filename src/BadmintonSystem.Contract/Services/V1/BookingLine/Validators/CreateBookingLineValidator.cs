using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.BookingLine.Validators;

public sealed class CreateBookingLineValidator : AbstractValidator<Command.CreateBookingLineCommand>
{
    public CreateBookingLineValidator()
    {
        RuleFor(x => x.Data.TotalPrice).NotEmpty().WithMessage("TotalPrice not null or empty");

        RuleFor(x => x.Data.YardId).NotEmpty().WithMessage("YardId not null or empty");

        RuleFor(x => x.Data.BookingId).NotEmpty().WithMessage("BookingId not null or empty");
    }
}
