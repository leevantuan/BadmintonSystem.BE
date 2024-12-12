using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.ReviewImage.Validators;

public sealed class DeleteReviewImagesValidator : AbstractValidator<Command.DeleteReviewImagesCommand>
{
    public DeleteReviewImagesValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
