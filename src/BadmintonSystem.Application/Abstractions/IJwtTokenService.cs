using System.Security.Claims;

namespace BadmintonSystem.Application.Abstractions;
public interface IJwtTokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
