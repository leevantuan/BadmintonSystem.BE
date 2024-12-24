using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.FixedSchedule.Validators;

public sealed class DeleteFixedScheduleValidator : AbstractValidator<Command.DeleteFixedSchedulesCommand>
{
    public DeleteFixedScheduleValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
