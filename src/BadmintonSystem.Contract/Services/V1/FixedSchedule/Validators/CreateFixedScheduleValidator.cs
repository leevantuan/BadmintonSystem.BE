using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.FixedSchedule.Validators;

public sealed class CreateFixedScheduleValidator : AbstractValidator<Command.CreateFixedScheduleCommand>
{
    public CreateFixedScheduleValidator()
    {
        RuleFor(x => x.Data.UserId).NotEmpty().WithMessage("User Id not null");

        RuleFor(x => x.Data.StartDate).NotEmpty().WithMessage("Start Date not null");

        RuleFor(x => x.Data.EndDate).NotEmpty().WithMessage("End Date not null");
    }
}
