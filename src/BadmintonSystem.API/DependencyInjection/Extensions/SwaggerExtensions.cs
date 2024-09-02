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
