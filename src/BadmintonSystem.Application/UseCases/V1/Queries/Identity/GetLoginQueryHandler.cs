using System.Security.Claims;
using AutoMapper;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Application.Extensions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Identity;

public sealed class GetLoginQueryHandler(
    IJwtTokenService jwtTokenService,
    IMapper mapper,
    UserManager<AppUser> userManager)
    : IQueryHandler<Query.LoginQuery, Response.LoginResponse>
{
    public async Task<Result<Response.LoginResponse>> Handle
        (Query.LoginQuery request, CancellationToken cancellationToken)
    {
        // Check user exists
        AppUser userByEmail = await userManager.FindByEmailAsync(request.Email)
                              ?? throw new IdentityException.AppUserNotFoundException(request.Email);

        bool isPasswordValid = await userManager.CheckPasswordAsync(userByEmail, request.Password);

        if (!isPasswordValid)
        {
            throw new IdentityException.AppUserException("Password invalid");
        }

        IList<string> roles = await userManager.GetRolesAsync(userByEmail);

        // Generate JWT Token
        Claim[] claims =
        {
            new(ClaimTypes.Email, userByEmail.Email),
            new(ClaimTypes.GivenName, userByEmail.FullName),
            new(ClaimTypes.Role, JsonConvert.SerializeObject(roles)),
            new(ClaimTypes.NameIdentifier, userByEmail.Id.ToString()),
            new(ClaimTypes.Name, userByEmail.UserName)
        };

        string accessToken = jwtTokenService.GenerateAccessToken(claims);

        string refreshToken = jwtTokenService.GenerateRefreshToken();

        Contract.Services.V1.User.Response.AppUserResponse? user =
            mapper.Map<Contract.Services.V1.User.Response.AppUserResponse>(userByEmail);

        var authValues = new List<Response.UserAuthorization>();

        var response = new Response.LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5),
            User = user
        };

        IList<Claim>? claimsUser = await userManager.GetClaimsAsync(userByEmail);

        if (claimsUser == null || !claimsUser.Any())
        {
            return Result.Success(response);
        }

        var functionKeys = Enum.GetValues<FunctionEnum>()
            .Select(e => e.ToString())
            .ToList();

        foreach (string function in functionKeys)
        {
            try
            {
                string? value = claimsUser.FirstOrDefault(x => x.Type == function)?.Value;
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

        response.Authorizations = authValues;

        return Result.Success(response);
    }
}
