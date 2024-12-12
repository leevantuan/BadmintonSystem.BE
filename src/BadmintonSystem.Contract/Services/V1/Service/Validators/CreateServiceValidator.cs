using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Service.Validators;
public sealed class CreateServiceValidator :
    AbstractValidator<Command.CreateServiceCommand>
{
    public CreateServiceValidator()
    {
        RuleFor(x => x.Data.Name).NotEmpty().WithMessage("Name not null")
            .MaximumLength(200).WithMessage("Name maximum 200 character");

        RuleFor(x => x.Data.SellingPrice).NotEmpty().WithMessage("Selling price not null")
            .GreaterThan(0).WithMessage("Selling price must greater than 0");

        RuleFor(x => x.Data.PurchasePrice).NotEmpty().WithMessage("Purchase price not null")
            .GreaterThan(0).WithMessage("Purchase price must greater than 0");

        RuleFor(x => x.Data.CategoryId).NotEmpty().WithMessage("CategoryId not null");

        RuleFor(x => x.Data.ClubId).NotEmpty().WithMessage("ClubId not null");
    }
}
