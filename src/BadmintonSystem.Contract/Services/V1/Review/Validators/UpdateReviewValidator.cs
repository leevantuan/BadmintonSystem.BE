using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Review.Validators;

public sealed class UpdateReviewValidator : AbstractValidator<Command.UpdateReviewCommand>
{
    public UpdateReviewValidator()
    {
        RuleFor(x => x.Data.Id).NotEmpty().WithMessage("Id not null");

        RuleFor(x => x.Data.Comment).NotEmpty().WithMessage("Comment not null");

        RuleFor(x => x.Data.RatingValue).NotEmpty().WithMessage("Rating value not null");

        RuleFor(x => x.Data.UserId).NotEmpty().WithMessage("User Id not null");
    }
}
