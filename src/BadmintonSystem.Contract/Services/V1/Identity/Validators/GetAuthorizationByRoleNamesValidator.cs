using FluentValidation;

namespace BadmintonSystem.Contract.Services.V1.Identity.Validators;

public sealed class GetAuthorizationByRoleNamesValidator : AbstractValidator<Query.GetAuthorizationByRoleNamesQuery>
{
    public GetAuthorizationByRoleNamesValidator()
    {
        RuleFor(x => x.RoleNames).NotEmpty().WithMessage("RoleNames not null or empty");
    }
}
