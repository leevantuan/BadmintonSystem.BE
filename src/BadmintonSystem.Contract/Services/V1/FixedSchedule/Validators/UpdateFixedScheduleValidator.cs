using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.FixedSchedule.Validators;

public sealed class UpdateFixedScheduleValidator : AbstractValidator<Command.UpdateFixedScheduleCommand>
{
    public UpdateFixedScheduleValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");
    }
}
