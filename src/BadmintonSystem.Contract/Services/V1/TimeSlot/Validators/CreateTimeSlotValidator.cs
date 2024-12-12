using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.TimeSlot.Validators;

public sealed class CreateTimeSlotValidator : AbstractValidator<Command.CreateTimeSlotCommand>
{
    public CreateTimeSlotValidator()
    {
        RuleFor(x => x.Data.StartTime).NotEmpty().WithMessage("StartTime not null or empty");

        RuleFor(x => x.Data.EndTime).NotEmpty().WithMessage("EndTime not null or empty");
    }
}
