using System.Security.Claims;
using AutoMapper;
using BadmintonSystem.Application.Extensions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
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

        var authValues = new List<Response.UserAuthorization>();

        AppUser user = await userManager.FindByEmailAsync(request.Email)
                       ?? throw new IdentityException.AppUserException(request.Email);

        result.User = mapper.Map<Contract.Services.V1.User.Response.AppUserResponse>(user);

        IList<Claim>? claims = await userManager.GetClaimsAsync(user);

        if (claims == null || !claims.Any())
        {
            return Result.Success(result);
        }

        var functionKeys = Enum.GetValues<FunctionEnum>()
            .Select(e => e.ToString())
            .ToList();

        foreach (string function in functionKeys)
        {
            try
            {
                string? value = claims.FirstOrDefault(x => x.Type == function)?.Value;
                Response.UserAuthorization? authValue = HandleActionExtension.ActionHandler(function, value);

                if (authValue != null)
                {
                    authValues.Add(authValue);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        result.Authorizations = authValues;

        return Result.Success(result);
    }
}
