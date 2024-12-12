using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.ReviewImage.Validators;

public sealed class UpdateReviewImageValidator : AbstractValidator<Command.UpdateReviewImageCommand>
{
    public UpdateReviewImageValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");

        RuleFor(x => x.Data.ImageLink).NotEmpty().WithMessage("Image not null");

        RuleFor(x => x.Data.ReviewId).NotEmpty().WithMessage("Review Id not null");
    }
}
