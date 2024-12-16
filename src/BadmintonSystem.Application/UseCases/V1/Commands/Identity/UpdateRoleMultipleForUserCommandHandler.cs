using System.Text;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;

public sealed class UpdateRoleMultipleForUserCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    ApplicationDbContext context)
    : ICommandHandler<Command.UpdateRoleMultipleForUserCommand>
{
    public async Task<Result> Handle
        (Command.UpdateRoleMultipleForUserCommand request, CancellationToken cancellationToken)
    {
        AppUser user = await userManager.FindByEmailAsync(request.Data.Email)
                       ?? throw new IdentityException.AppUserNotFoundException(request.Data.Email);

        // Get all Roles of User
        IList<string> userRoles = await userManager.GetRolesAsync(user);

        var rolesToRemove = userRoles
            .Where(currentRole =>
                !request.Data.Roles.Any(requestRole =>
                    string.Equals(
                        StringExtension.CapitalizeFirstLetter(currentRole.Trim()),
                        StringExtension.CapitalizeFirstLetter(requestRole.Trim()),
                        StringComparison.OrdinalIgnoreCase)))
            .ToList();

        var removeQueryBuilder = new StringBuilder();

        removeQueryBuilder.Append($@"DELETE FROM ""{nameof(AppUserRole)}s""
                                         WHERE ""{nameof(AppUserRole.UserId)}"" = '{user.Id}'
                                         AND ""{nameof(AppUserRole.RoleId)}""::TEXT ILIKE ANY (ARRAY[ ");

        // Remove Roles
        foreach (string userRole in rolesToRemove)
        {
            string roleName = StringExtension.CapitalizeFirstLetter(userRole.Trim());

            AppRole? role = await roleManager.FindByNameAsync(roleName);

            removeQueryBuilder.Append($@"'%{role.Id}%', ");

            // if (!await userManager.IsInRoleAsync(user, roleName))
            // {
            //     continue;
            // }
            //
            // IdentityResult removeRoleResult = await userManager.RemoveFromRoleAsync(user, roleName);
            // if (!removeRoleResult.Succeeded)
            // {
            //     string errors = string.Join(", ", removeRoleResult.Errors.Select(e => e.Description));
            //     throw new IdentityException.AppRoleException(errors);
            // }
        }

        removeQueryBuilder.Length -= 2;

        removeQueryBuilder.Append("]) ");

        context.Database.ExecuteSqlRaw(removeQueryBuilder.ToString());

        await context.SaveChangesAsync(cancellationToken);

        // Check if not have role then Add Role
        foreach (string item in request.Data.Roles)
        {
            string roleName = StringExtension.Uppercase(item);

            string? existsRole = userRoles.FirstOrDefault(x => x.ToLower().Equals(roleName.ToLower()));

            if (existsRole == null)
            {
                IdentityResult addRoleOfUserResult = await userManager.AddToRoleAsync(user, roleName);
                if (!addRoleOfUserResult.Succeeded)
                {
                    string errors = string.Join(", ", addRoleOfUserResult.Errors.Select(e => e.Description));

                    throw new IdentityException.AppRoleException(errors);
                }
            }
        }

        return Result.Success();
    }
}
