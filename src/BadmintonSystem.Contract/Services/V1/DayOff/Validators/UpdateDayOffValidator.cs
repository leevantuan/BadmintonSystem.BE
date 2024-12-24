using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.DayOff.Validators;

public sealed class UpdateDayOffValidator : AbstractValidator<Command.UpdateDayOffCommand>
{
    public UpdateDayOffValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");
    }
}
