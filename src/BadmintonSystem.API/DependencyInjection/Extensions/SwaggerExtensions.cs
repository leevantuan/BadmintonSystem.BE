using BadmintonSystem.API.DependencyInjection.Options;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        // Add Sawagger Gen in Program to here
        services.AddSwaggerGen();
        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Badminton", Version = "v1" });
        //    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        //    {
        //        Description = "Input your API key",
        //        In = ParameterLocation.Header,
        //        Name = "Authorization",
        //        Type = SecuritySchemeType.ApiKey
        //    });
        //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        //    {
        //        {
        //            new OpenApiSecurityScheme
        //            {
        //                Reference = new OpenApiReference
        //                {
        //                    Type = ReferenceType.SecurityScheme,
        //                    Id = "ApiKey"
        //                }
        //            },
        //            new List<string>()
        //        }
        //    });
        //});

        #region ========================= SWAGGER GEN "Bearer" ====================

        //services.AddSwaggerGen(c =>
        //{
        //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Smart", Version = "v1" });

        //    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //    {
        //        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
        //              Enter 'Bearer' [space] and then your token in the text input below.
        //              \r\n\r\nExample: 'Bearer 12345abcdef'",
        //        Name = "Authorization",
        //        In = ParameterLocation.Header,
        //        Type = SecuritySchemeType.ApiKey,
        //        Scheme = "Bearer"
        //    });

        //    c.AddSecurityRequirement(new OpenApiSecurityRequirement(){{ new OpenApiSecurityScheme {
        //                                                            Reference = new OpenApiReference
        //                                                                {
        //                                                                Type = ReferenceType.SecurityScheme,
        //                                                                Id = "Bearer"
        //                                                                },
        //                                                                Scheme = "oauth2",
        //                                                                Name = "Bearer",
        //                                                                In = ParameterLocation.Header,
        //                                                            },
        //                                                            new List<string>()
        //                                                            }
        //                                                        });
        //});

        #endregion ========================= SWAGGER GEN "Bearer" ====================

        // Config thêm cho SwaggerGen
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    }

    // After add SwaggerGen then config it
    public static void ConfigureSwagger(this WebApplication app)
    {
        app.UseSwagger(); // Có sử dụng hay không
        app.UseSwaggerUI(options =>
        {
            // Loop qua tất cả các version
            foreach (var version in app.DescribeApiVersions().Select(version => version.GroupName))
                options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version); // Tự động Add các version

            options.DisplayRequestDuration();
            options.EnableTryItOutByDefault();
            options.DocExpansion(DocExpansion.None);
        });

        app.MapGet("/", () => Results.Redirect("/swagger/index.html"))
            .WithTags(string.Empty);
    }
}
