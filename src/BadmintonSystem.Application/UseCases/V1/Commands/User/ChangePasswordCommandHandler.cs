using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.User;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.User;

public sealed class ChangePasswordCommandHandler(
    UserManager<AppUser> userManager)
    : ICommandHandler<Command.ChangePasswordCommand>
{
    public async Task<Result> Handle(Command.ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        AppUser? userById = await userManager.FindByIdAsync(request.Data.Id.ToString())
                            ?? throw new IdentityException.AppUserNotFoundException(request.Data.Id);

        bool isPasswordValid = await userManager.CheckPasswordAsync(userById, request.Data.CurrentPassword);

        if (!isPasswordValid)
        {
            throw new PasswordException.PasswordNotMatchException();
        }

        bool confirmPassword = PasswordExtension.ConfirmPassword(request.Data.Password, request.Data.ConfirmPassword);

        if (!confirmPassword)
        {
            throw new PasswordException.PasswordNotConfirmException();
        }

        IdentityResult result =
            await userManager.ChangePasswordAsync(userById, request.Data.CurrentPassword, request.Data.Password);

        if (!result.Succeeded)
        {
            string errors = string.Join(", ", result.Errors.Select(e => e.Description));

            throw new IdentityException.AppUserException(errors);
        }

        return Result.Success();
    }
}
