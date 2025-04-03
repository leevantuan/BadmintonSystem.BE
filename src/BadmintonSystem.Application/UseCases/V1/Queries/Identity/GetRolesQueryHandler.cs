using System.Security.Claims;
using BadmintonSystem.Application.Extensions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Identity;

public sealed class GetRolesQueryHandler(
    RoleManager<AppRole> roleManager)
    : IQueryHandler<Query.GetRolesQuery, List<Response.RoleAuthorization>>
{
    public async Task<Result<List<Response.RoleAuthorization>>> Handle(Query.GetRolesQuery request, CancellationToken cancellationToken)
    {
        var results = new List<Response.RoleAuthorization>();
        var roleNames = await roleManager.Roles.Select(r => r.Name).ToListAsync(cancellationToken)
            ?? throw new ApplicationException("Roles not found");

        // ROLE CLAIMS
        foreach (string roleName in roleNames)
        {
            string roleNameCapitalize = StringExtension.Uppercase(roleName);

            var result = new Response.RoleAuthorization
            {
                RoleName = roleNameCapitalize
            };

            AppRole? role = await roleManager.FindByNameAsync(roleNameCapitalize)
                            ?? throw new IdentityException.AppRoleException(roleNameCapitalize);

            IList<Claim> roleClaim = await roleManager.GetClaimsAsync(role);

            result.Authorizations = AuthenticationExtension.GetActionValues(roleClaim);

            results.Add(result);
        }

        return Result.Success(results);
    }
}
