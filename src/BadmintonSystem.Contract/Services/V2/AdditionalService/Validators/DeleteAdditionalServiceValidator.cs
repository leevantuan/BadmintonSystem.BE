using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.AdditionalService.Validators;
public class DeleteAdditionalServiceValidator : AbstractValidator<Command.DeleteAdditionalServiceCommand>
{
    public DeleteAdditionalServiceValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
