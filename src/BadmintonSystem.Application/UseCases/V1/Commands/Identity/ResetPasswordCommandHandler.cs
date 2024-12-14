using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;

public sealed class ResetPasswordCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    ApplicationDbContext context)
    : ICommandHandler<Command.ResetPasswordByIdCommand>
{
    public async Task<Result> Handle(Command.ResetPasswordByIdCommand request, CancellationToken cancellationToken)
    {
        AppUser? userById = await userManager.FindByIdAsync(request.Data.Id.ToString())
                            ?? throw new IdentityException.AppUserNotFoundException(request.Data.Id);

        IdentityResult removeResult = await userManager.RemovePasswordAsync(userById);

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

        IdentityResult addResult = await userManager.AddPasswordAsync(userById, request.Data.Password);

        if (!addResult.Succeeded)
        {
            string errors = string.Join(", ", addResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppUserException(errors);
        }

        // string token = await userManager.GeneratePasswordResetTokenAsync(userById);
        //
        // string resetToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        //
        // IdentityResult result = await userManager.ResetPasswordAsync(userById, resetToken, request.Data.Password);

        return Result.Success();
    }
}
