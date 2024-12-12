using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Notification.Validators;

public sealed class DeleteNotificationsValidator : AbstractValidator<Command.DeleteNotificationsCommand>
{
    public DeleteNotificationsValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null or empty");
    }
}
