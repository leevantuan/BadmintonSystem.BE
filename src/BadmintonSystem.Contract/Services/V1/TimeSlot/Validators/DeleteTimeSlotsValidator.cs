using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.TimeSlot.Validators;

public sealed class DeleteTimeSlotsValidator : AbstractValidator<Command.DeleteTimeSlotsCommand>
{
    public DeleteTimeSlotsValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null or empty");
    }
}
