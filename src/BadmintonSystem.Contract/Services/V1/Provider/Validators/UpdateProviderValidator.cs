using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Provider.Validators;

public sealed class UpdateProviderValidator : AbstractValidator<Command.UpdateProviderCommand>
{
    public UpdateProviderValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");
    }
}
