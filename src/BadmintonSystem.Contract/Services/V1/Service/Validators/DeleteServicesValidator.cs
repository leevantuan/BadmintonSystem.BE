using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Service.Validators;

public sealed class DeleteServicesValidator :
    AbstractValidator<Command.DeleteServicesCommand>
{
    public DeleteServicesValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
