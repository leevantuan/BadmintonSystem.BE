using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Yard.Validators;

public sealed class UpdateYardValidator : AbstractValidator<Command.UpdateYardCommand>
{
    public UpdateYardValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");

        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null");

        RuleFor(x => x.Data.YardTypeId).NotEmpty().WithMessage("Yard type Id not null");
    }
}
