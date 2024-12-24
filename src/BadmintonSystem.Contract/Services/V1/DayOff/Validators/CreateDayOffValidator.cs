using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.DayOff.Validators;

public sealed class CreateDayOffValidator : AbstractValidator<Command.CreateDayOffCommand>
{
    public CreateDayOffValidator()
    {
        RuleFor(x => x.Data.Date).NotEmpty().WithMessage("Date not null");

        RuleFor(x => x.Data.Content).NotEmpty().WithMessage("Content not null");
    }
}
