using System.Security.Claims;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Identity;
public sealed class GetLoginQueryHandler : IQueryHandler<Query.LoginQuery, Response.Authenticated>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly UserManager<AppUser> _userManager;

    public GetLoginQueryHandler(IJwtTokenService jwtTokenService, UserManager<AppUser> userManager)
    {
        _jwtTokenService = jwtTokenService;
        _userManager = userManager;
    }

    public async Task<Result<Response.Authenticated>> Handle(Query.LoginQuery request, CancellationToken cancellationToken)
    {
        // Check user exists
        var userByEmail = await _userManager.FindByEmailAsync(request.Email)
                    ?? throw new IdentityException.AppUserNotFoundException(request.Email);

        var isPasswordValid = await _userManager.CheckPasswordAsync(userByEmail, request.Password);

        if (!isPasswordValid)
        {
            throw new IdentityException.AppUserException("Password invalid");
        }

        var roles = await _userManager.GetRolesAsync(userByEmail);

        // Generate JWT Token
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, userByEmail.Email),
            new Claim(ClaimTypes.GivenName, userByEmail.FullName),
            new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(roles)),
            new Claim(ClaimTypes.NameIdentifier, userByEmail.Id.ToString()),
            new Claim(ClaimTypes.Name, userByEmail.UserName)
        };

        var accessToken = _jwtTokenService.GenerateAccessToken(claims);

        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        var response = new Response.Authenticated()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5)
        };

        return Result.Success(response);
    }
}
