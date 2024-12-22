using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.YardPrice.Validators;

public sealed class CreateYardPriceValidator : AbstractValidator<Command.CreateYardPriceCommand>
{
    public CreateYardPriceValidator()
    {
        RuleFor(x => x.Data.YardId).NotEmpty().WithMessage("Yard Id not null");

        RuleFor(x => x.Data.PriceId).NotEmpty().WithMessage("Price Id not null");

        RuleFor(x => x.Data.TimeSlotId).NotEmpty().WithMessage("TimeSlot Id not null");

        RuleFor(x => x.Data.EffectiveDate).NotEmpty().WithMessage("EffectiveDate not null");

        RuleFor(x => x.Data.IsBooking).NotEmpty().WithMessage("Is Booking not null");
    }
}
