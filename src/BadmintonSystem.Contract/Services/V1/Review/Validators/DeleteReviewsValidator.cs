using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Review.Validators;

public sealed class DeleteReviewsValidator : AbstractValidator<Command.DeleteReviewsCommand>
{
    public DeleteReviewsValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().WithMessage("Ids not null");
    }
}
