using FluentValidation;

namespace BadmintonSystem.Contract.Services.V2.Club.Validators;
public class DeleteClubValidator : AbstractValidator<Command.DeleteClubCommand>
{
    public DeleteClubValidator()
    {
        RuleFor(x => x.Id).NotEmpty(); // không null
    }
}
