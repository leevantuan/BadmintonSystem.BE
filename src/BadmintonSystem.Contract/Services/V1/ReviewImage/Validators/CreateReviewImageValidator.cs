using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.ReviewImage.Validators;

public sealed class CreateReviewImageValidator : AbstractValidator<Command.CreateReviewImageCommand>
{
    public CreateReviewImageValidator()
    {
        RuleFor(x => x.Data.ImageLink).NotEmpty().WithMessage("Comment not null");

        RuleFor(x => x.Data.ReviewId).NotEmpty().WithMessage("Review Id not null");
    }
}
