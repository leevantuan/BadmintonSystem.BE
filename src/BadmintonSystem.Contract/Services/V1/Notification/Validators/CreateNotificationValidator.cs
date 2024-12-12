using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Notification.Validators;

public sealed class CreateNotificationValidator : AbstractValidator<Command.CreateNotificationCommand>
{
    public CreateNotificationValidator()
    {
        RuleFor(x => x.Data.Content).NotEmpty().WithMessage("Content not null or empty");

        RuleFor(x => x.Data.UserId).NotEmpty().WithMessage("UserId not null or empty");
    }
}
