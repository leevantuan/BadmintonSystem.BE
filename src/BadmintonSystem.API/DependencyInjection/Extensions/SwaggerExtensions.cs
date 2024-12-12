using Asp.Versioning.ApiExplorer;
using BadmintonSystem.API.DependencyInjection.Options;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BadmintonSystem.API.DependencyInjection.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerAPI(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Retrieve the version description provider
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                c.SwaggerDoc(description.GroupName, new OpenApiInfo()
                {
                    Title = AppDomain.CurrentDomain.FriendlyName,
                    Version = description.ApiVersion.ToString()
                });
            }

            // Configure security definition for Bearer authentication
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Input your token!. ",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            // Set up the requirement for the Bearer token in requests
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "Bearer",
                        Name = "Authorization",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    }

    public static void AddSwaggerConfigurationAPI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var version in app.DescribeApiVersions().Select(version => version.GroupName))
                options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);

            options.DisplayRequestDuration();
            options.EnableTryItOutByDefault();
            options.DocExpansion(DocExpansion.None);
        });

        app.MapGet("/", () => Results.Redirect("/swagger/index.html"))
            .WithTags(string.Empty);
    }
}
