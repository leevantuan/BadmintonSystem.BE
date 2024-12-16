using System.Security.Claims;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Identity;

public sealed class CreateAppRoleCommandHandler(
    RoleManager<AppRole> roleManager,
    ApplicationDbContext context)
    : ICommandHandler<Command.CreateAppRoleCommand>
{
    public async Task<Result> Handle(Command.CreateAppRoleCommand request, CancellationToken cancellationToken)
    {
        // check exist
        AppRole? roleByRoleCode =
            await roleManager.Roles.FirstOrDefaultAsync(r => r.RoleCode == request.Data.RoleCode, cancellationToken);

        if (roleByRoleCode != null)
        {
            throw new IdentityException.AppRoleAlreadyExistException(roleByRoleCode.RoleCode);
        }

        var role = new AppRole
        {
            Name = request.Data.RoleCode.Trim(),
            RoleCode = request.Data.RoleCode.Trim().ToUpper(),
            Description = request.Data.Description.Trim()
        };

        await roleManager.CreateAsync(role);

        await context.SaveChangesAsync(cancellationToken);

        IList<Claim> roleClaims = await roleManager.GetClaimsAsync(role);

        if (!roleClaims.Any())
        {
            foreach (FunctionEnum functionEnum in Enum.GetValues(typeof(FunctionEnum)))
            {
                string functionName = functionEnum.ToString().Trim().ToUpper();

                await AddAppRoleClaim(functionName, "0", role, context, roleManager);
            }
        }

        return Result.Success();
    }

    private static async Task AddAppRoleClaim
    (string functionName, string value,
        AppRole role, ApplicationDbContext context, RoleManager<AppRole> roleManager)
    {
        var claim = new Claim(functionName, value);

        await roleManager.AddClaimAsync(role, claim);
        await context.SaveChangesAsync();
    }
}
