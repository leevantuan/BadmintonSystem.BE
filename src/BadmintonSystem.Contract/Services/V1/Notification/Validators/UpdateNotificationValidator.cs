using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Notification.Validators;

public sealed class UpdateNotificationValidator : AbstractValidator<Command.UpdateNotificationCommand>
{
    public UpdateNotificationValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null or empty");

        RuleFor(x => x.Data.Content).NotEmpty().WithMessage("Content not null or empty");

        RuleFor(x => x.Data.UserId).NotEmpty().WithMessage("UserId not null or empty");
    }
}
