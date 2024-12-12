using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Sale.Validators;

public sealed class CreateSaleValidator : AbstractValidator<Command.CreateSaleCommand>
{
    public CreateSaleValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null or empty");

        RuleFor(x => x.Data.Percent).NotEmpty().WithMessage("Percent not null or empty");

        RuleFor(x => x.Data.StartDate).NotEmpty().WithMessage("Start date not null or empty");

        RuleFor(x => x.Data.EndDate).NotEmpty().WithMessage("End date not null or empty");

        RuleFor(x => x.Data.IsActive).NotEmpty().WithMessage("Status not null or empty");
    }
}
