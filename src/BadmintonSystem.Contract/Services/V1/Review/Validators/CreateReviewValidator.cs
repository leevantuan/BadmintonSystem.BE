using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Review.Validators;

public sealed class CreateReviewValidator : AbstractValidator<Command.CreateReviewCommand>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.Data.Comment).NotEmpty().WithMessage("Comment not null");

        RuleFor(x => x.Data.RatingValue).NotEmpty().WithMessage("Rating value not null");

        RuleFor(x => x.Data.UserId).NotEmpty().WithMessage("User Id not null");
    }
}
