using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using static BadmintonSystem.Contract.Services.V1.Identity.Command;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;
public sealed class ResetUserToDefaultRoleCommandHandler : ICommandHandler<Command.ResetUserToDefaultRoleCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public ResetUserToDefaultRoleCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(ResetUserToDefaultRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Data.Email)
            ?? throw new IdentityException.AppUserNotFoundException(request.Data.Email);

        // Remove all roles
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            await _userManager.RemoveFromRoleAsync(user, userRole);
        }

        // Remove all claims
        var userClaims = await _userManager.GetClaimsAsync(user);
        foreach (var claim in userClaims)
        {
            await _userManager.RemoveClaimAsync(user, claim);
        }

        var roleName = StringExtension.CapitalizeFirstLetter(request.Data.RoleName);
        var role = await _roleManager.FindByNameAsync(roleName)
            ?? throw new IdentityException.AppRoleNotFoundException(request.Data.RoleName);

        var addDefaultRoleOfUserResult = await _userManager.AddToRoleAsync(user, roleName);

        if (!addDefaultRoleOfUserResult.Succeeded)
        {
            var errors = string.Join(", ", addDefaultRoleOfUserResult.Errors.Select(e => e.Description));

            throw new IdentityException.AppRoleException(errors);
        }

        await SetUserClaimWithRoleClaim(role, user);

        return Result.Success();
    }

    private async Task SetUserClaimWithRoleClaim(AppRole role, AppUser newUser)
    {
        var roleClaims = await _roleManager.GetClaimsAsync(role);

        await _userManager.AddClaimsAsync(newUser, roleClaims);
    }
}
