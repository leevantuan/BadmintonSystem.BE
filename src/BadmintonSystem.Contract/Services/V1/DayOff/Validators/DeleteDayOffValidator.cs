using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.DayOff.Validators;

public sealed class DeleteDayOffValidator : AbstractValidator<Command.DeleteDayOffsCommand>
{
    public DeleteDayOffValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
