using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Infrastructure.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BadmintonSystem.Infrastructure.Authentication;
public class JwtTokenService : IJwtTokenService
{
    // get option
    private readonly JwtOption jwtOption = new JwtOption();

    public JwtTokenService(IConfiguration configuration)
    {
        configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenOptions = new JwtSecurityToken(
            issuer: jwtOption.Issuer,
            audience: jwtOption.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtOption.ExpireMin),
            signingCredentials: signingCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        // server
        var Key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);

        var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false, // on production make it true
            ValidateAudience = false, // on production make it true
            ValidateLifetime = false, // here we are saying that we don't care about the token's expiration date
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOption.Issuer,
            ValidAudience = jwtOption.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        // check token expired is a token you generate or not ? => because you set ValidateLifeTime = false (server check)
        // if true (token you generate right) => return principle
        var principle = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)securityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principle;
    }
}
