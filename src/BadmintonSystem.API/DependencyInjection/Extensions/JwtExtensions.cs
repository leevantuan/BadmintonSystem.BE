using System.Text;
using BadmintonSystem.Infrastructure.DependencyInjection.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

public static class JwtExtensions
{
    public static void AddJwtAuthenticationConfigurationAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                JwtOption jwtOption = new JwtOption();
                configuration.GetSection(nameof(JwtOption)).Bind(jwtOption);

                // config more
                var Key = Encoding.UTF8.GetBytes(jwtOption.SecretKey);
                options.SaveToken = true;
                // save token into Authentication Properties => so you can get access token by Authentication Properties
                // var accessToken = await HttpContext.GetTokenAsync("access_token")

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false, // on production make it true
                    ValidateAudience = false, // on production make it true
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOption.Issuer,
                    ValidAudience = jwtOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("IS-TOKEN-EXPIRED", true.ToString());
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
    }
}
