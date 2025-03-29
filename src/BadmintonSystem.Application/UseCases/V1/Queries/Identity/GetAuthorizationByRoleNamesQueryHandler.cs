using System.Security.Claims;
using BadmintonSystem.Application.Extensions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Identity;

public sealed class GetAuthorizationByRoleNamesQueryHandler(
    RoleManager<AppRole> roleManager)
    : IQueryHandler<Query.GetAuthorizationByRoleNamesQuery, List<Response.RoleAuthorization>>
{
    public async Task<Result<List<Response.RoleAuthorization>>> Handle
        (Query.GetAuthorizationByRoleNamesQuery request, CancellationToken cancellationToken)
    {
        var results = new List<Response.RoleAuthorization>();

        // ROLE CLAIMS
        foreach (string roleName in request.RoleNames)
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
