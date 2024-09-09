using System.Text;
using BadmintonSystem.Infrastructure.DependencyInjection.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

// This file is Validate hoặc Middleware cho JWt
public static class JwtEntensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Default "Bearer"
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            JwtOption jwtOption = new JwtOption();
            configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);

            var Key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);
            opt.SaveToken = true; // Save token vào trong Authentication properties có thể get bằng Accesstoken
            // Example: var actoken = await HttpContext.getTokenAsync("name_token")

            // Validate
            // JWT : Header thuật toán , JWT type
            // JWT : PayLoad Các Claims truyền vào
            // JWT : Signature Kết hợp vào xác thực Key + Header + Payload ==> Generate
            opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOption.Issuer,
                ValidAudience = jwtOption.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero,
            };

            // Nếu nó Fial thì sẽ Add thêm cái Header và response trả về
            // Token Expired đã hết hạn "true"
            opt.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                    }

                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();
    }
}
