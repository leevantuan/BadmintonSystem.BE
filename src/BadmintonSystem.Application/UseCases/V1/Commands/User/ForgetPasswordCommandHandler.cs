using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.User;

public sealed class ForgetPasswordCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    ApplicationDbContext context)
    : ICommandHandler<Command.ForgetPasswordCommand>
{
    public async Task<Result> Handle(Command.ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        AppUser? userByEmail = await userManager.FindByEmailAsync(request.Data.Email)
                               ?? throw new IdentityException.AppUserNotFoundException(request.Data.Email);

        IdentityResult removeResult = await userManager.RemovePasswordAsync(userByEmail);

        if (!removeResult.Succeeded)
        {
            string errors = string.Join(", ", removeResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppUserException(errors);
        }

        bool confirmPassword = PasswordExtension.ConfirmPassword(request.Data.Password, request.Data.ConfirmPassword);

        if (!confirmPassword)
        {
            throw new PasswordException.PasswordNotConfirmException();
        }

        IdentityResult addResult = await userManager.AddPasswordAsync(userByEmail, request.Data.Password);

        if (!addResult.Succeeded)
        {
            string errors = string.Join(", ", addResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppUserException(errors);
        }

        return Result.Success();
    }
}
