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
    private readonly JwtOption jwtOption = new JwtOption();

    public JwtTokenService(IConfiguration configuration)
    {
        configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        // Phải giống nhau thì mới Xác thực được
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption.SecretKey));

        // Thuật toán Hash 256
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        // Các option để Generate
        var tokenOptions = new JwtSecurityToken(
            issuer: jwtOption.Issuer, // False nếu không giống vẫn chạy
            audience: jwtOption.Audience, // False nếu không giống vẫn chạy
            claims: claims,
            expires: DateTime.Now.AddMinutes(jwtOption.ExpireMin),
            signingCredentials: signinCredentials);

        // Generate token
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        // Random ra các số và chữ, ký tự đặc biệt
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);

            // Convert base 64
            return Convert.ToBase64String(randomNumber);
        }
    }

    // Kiểm tra có hợp lệ không
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var Key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);

        // Kiểm tra xem cso đúng với token cấp phát hay không
        var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false, // Bỏ qua thời hạn xử lý token
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ClockSkew = TimeSpan.Zero,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        // Nếu đúng thì sẽ lấy các Claim, token
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Token không hợp lệ");

            return principal;
        }
        catch (Exception ex)
        {
            throw new SecurityTokenException("Token không hợp lệ hoặc đã bị lỗi", ex);
        }
    }
}
