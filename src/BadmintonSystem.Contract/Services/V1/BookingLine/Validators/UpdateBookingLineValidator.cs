using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.BookingLine.Validators;

public sealed class UpdateBookingLineValidator : AbstractValidator<Command.UpdateBookingLineCommand>
{
    public UpdateBookingLineValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null or empty");

        // RuleFor(x => x.Data.TotalPrice).NotEmpty().WithMessage("TotalPrice not null or empty");
        //
        // RuleFor(x => x.Data.YardId).NotEmpty().WithMessage("YardId not null or empty");
        //
        // RuleFor(x => x.Data.BookingId).NotEmpty().WithMessage("BookingId not null or empty");
    }
}
