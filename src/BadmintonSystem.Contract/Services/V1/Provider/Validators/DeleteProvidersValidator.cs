using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Provider.Validators;

public sealed class DeleteProvidersValidator : AbstractValidator<Command.DeleteProvidersCommand>
{
    public DeleteProvidersValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
