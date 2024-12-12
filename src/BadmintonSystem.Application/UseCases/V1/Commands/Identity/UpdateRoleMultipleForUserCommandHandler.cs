using System.Security.Claims;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;
public sealed class UpdateRoleMultipleForUserCommandHandler : ICommandHandler<Command.UpdateRoleMultipleForUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public UpdateRoleMultipleForUserCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Result> Handle(Command.UpdateRoleMultipleForUserCommand request, CancellationToken cancellationToken)
    {
        Dictionary<string, string>? newRoles = new Dictionary<string, string>();

        var user = await _userManager.FindByEmailAsync(request.Data.Email)
            ?? throw new IdentityException.AppUserNotFoundException(request.Data.Email);

        var userClaims = await _userManager.GetClaimsAsync(user);
        newRoles = userClaims.ToDictionary(item => item.Type, item => item.Value) ?? new Dictionary<string, string>();

        // Get all Roles of User
        var userRoles = await _userManager.GetRolesAsync(user);

        // Remove Roles
        foreach (var userRole in userRoles)
        {
            var roleName = StringExtension.CapitalizeFirstLetter(userRole);

            var existRoleInRequest = request.Data.Roles.Find(x => x.Trim().ToLower().Equals(userRole.Trim().ToLower()));
            if (existRoleInRequest == null)
            {
                var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (!removeRoleResult.Succeeded)
                {
                    var errors = string.Join(", ", removeRoleResult.Errors.Select(e => e.Description));

                    throw new IdentityException.AppRoleException(errors);
                }

                var roleClaims = await GetListRoleClaimsByName(roleName);
                newRoles = SplitRoleClaims(newRoles, roleClaims);
            }
        }

        // Check if not have role then Add Role
        foreach (var item in request.Data.Roles)
        {
            var roleName = StringExtension.CapitalizeFirstLetter(item);

            var existsRole = userRoles.FirstOrDefault(x => x.Equals(roleName));

            if (existsRole == null)
            {
                var addRoleOfUserResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!addRoleOfUserResult.Succeeded)
                {
                    var errors = string.Join(", ", addRoleOfUserResult.Errors.Select(e => e.Description));

                    throw new IdentityException.AppRoleException(errors);
                }

                var roleClaims = await GetListRoleClaimsByName(roleName);
                newRoles = MergeRoleClaims(newRoles, roleClaims);
            }
        }

        await SaveChanges(newRoles, userClaims, user);

        return Result.Success();
    }

    private async Task<IList<Claim>> GetListRoleClaimsByName(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName)
            ?? throw new IdentityException.AppRoleNotFoundException(roleName);

        var roleClaims = await _roleManager.GetClaimsAsync(role);

        return roleClaims;
    }

    private static Dictionary<string, string> MergeRoleClaims(Dictionary<string, string> newRoles, IList<Claim> roleClaims)
    {
        foreach (var item in roleClaims.ToList())
        {
            if (newRoles.TryGetValue(item.Type, out string value))
            {
                int.TryParse(item.Value, out int newValue);
                int.TryParse(value, out int oldValue);
                newValue |= oldValue;
                newRoles[item.Type] = newValue.ToString();
            }
            else
            {
                newRoles.TryAdd(item.Type, item.Value);
            }
        }

        return newRoles;
    }

    private static Dictionary<string, string> SplitRoleClaims(Dictionary<string, string> newRoles, IList<Claim> roleClaims)
    {
        foreach (var item in roleClaims.ToList())
        {
            if (newRoles.TryGetValue(item.Type, out string value))
            {
                int.TryParse(item.Value, out int removeValue);
                int.TryParse(value, out int oldValue);
                int newValue = oldValue &= ~removeValue;
                newRoles[item.Type] = newValue.ToString();
            }
        }

        return newRoles;
    }

    private async Task SaveChanges(Dictionary<string, string> mergeRoles, IList<Claim> userClaims, AppUser user)
    {
        foreach (var item in userClaims.ToList())
        {
            if (mergeRoles.TryGetValue(item.Type, out string newValue))
            {
                await _userManager.RemoveClaimAsync(user, item);

                var newClaim = new Claim(item.Type, newValue.ToString());
                await _userManager.AddClaimAsync(user, newClaim);

                mergeRoles.Remove(item.Type);
            }
        }

        foreach (var res in mergeRoles)
        {
            var newClaim = new Claim(res.Key.ToString().ToUpper(), res.Value.ToString());
            await _userManager.AddClaimAsync(user, newClaim);
        }
    }
}
