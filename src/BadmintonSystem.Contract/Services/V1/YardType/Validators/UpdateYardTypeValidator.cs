using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.YardType.Validators;

public sealed class UpdateYardTypeValidator : AbstractValidator<Command.UpdateYardTypeCommand>
{
    public UpdateYardTypeValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");

        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null");
    }
}
