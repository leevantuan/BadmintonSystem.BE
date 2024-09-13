using System.Security.Claims;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Authen;
using static BadmintonSystem.Contract.Services.V2.Authen.Command;
using static BadmintonSystem.Contract.Services.V2.Authen.Response;

namespace BadmintonSystem.Application.UseCases.V2.Identity;
public class GetLoginCommandHandler : ICommandHandler<Login, Authenticed>
{
    private readonly IJwtTokenService _jwtTokenService;

    public GetLoginCommandHandler(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<Authenticed>> Handle(Login request, CancellationToken cancellationToken)
    {
        // Check User Here

        // Generate JWT Token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, request.Email),
            new Claim(ClaimTypes.Role, "Adim_Test")
        };

        var accessToken = _jwtTokenService.GenerateAccessToken(claims);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        var response = new Response.Authenticed()
        {
            AccessToken = accessToken, // 2p cho AccessToken
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(5) // Cho refresh Token, Back to Login
        };

        return response;
    }
}
