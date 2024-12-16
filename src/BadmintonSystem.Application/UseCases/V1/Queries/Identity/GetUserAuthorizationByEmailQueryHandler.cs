using System.Security.Claims;
using AutoMapper;
using BadmintonSystem.Application.Extensions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Identity;

public sealed class GetUserAuthorizationByEmailQueryHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    IMapper mapper,
    ApplicationDbContext context)
    : IQueryHandler<Query.GetUserAuthorizationByEmailQuery, Response.UserDetailResponse>
{
    public async Task<Result<Response.UserDetailResponse>> Handle
    (
        Query.GetUserAuthorizationByEmailQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Response.UserDetailResponse();

        AppUser user = await userManager.FindByEmailAsync(request.Email)
                       ?? throw new IdentityException.AppUserException(request.Email);

        result.User = mapper.Map<Contract.Services.V1.User.Response.AppUserResponse>(user);

        IList<string> roles = await userManager.GetRolesAsync(user);

        result.Roles = roles.ToList();

        // USER CLAIMS
        IList<Claim>? userClaims = await userManager.GetClaimsAsync(user);

        // Initial Claim By UserClaim
        var resultClaims = userClaims.ToDictionary(item => item.Type, item => item.Value);

        // ROLE CLAIMS
        foreach (string roleName in roles)
        {
            string roleNameCapitalize = StringExtension.Uppercase(roleName);

            AppRole? role = await roleManager.FindByNameAsync(roleNameCapitalize)
                            ?? throw new IdentityException.AppRoleException(roleNameCapitalize);

            IList<Claim> roleClaim = await roleManager.GetClaimsAsync(role);

            // MERGE ROLE
            resultClaims = AuthenticationExtension.MergeClaims(resultClaims, roleClaim);
        }

        userClaims = resultClaims
            .Select(e => new Claim(e.Key, e.Value))
            .ToList();

        result.Authorizations = AuthenticationExtension.GetActionValues(userClaims);

        return Result.Success(result);
    }
}
