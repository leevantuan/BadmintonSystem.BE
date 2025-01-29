using BadmintonSystem.API.DependencyInjection.Extensions;
using BadmintonSystem.API.Hubs;
using BadmintonSystem.API.Middleware;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Application.DependencyInjection.Extensions;
using BadmintonSystem.Contract.Constants;
using BadmintonSystem.Infrastructure.DependencyInjection.Extensions;
using BadmintonSystem.Infrastructure.DependencyInjection.Options;
using BadmintonSystem.Infrastructure.Seed;
using BadmintonSystem.Infrastructure.Services;
using BadmintonSystem.Persistence.DependencyInjection.Extensions;
using BadmintonSystem.Persistence.DependencyInjection.Options;
using Carter;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Serilog;
using StackExchange.Redis;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add CORS policy to allow any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add Serilog
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();

// Add SignalR
builder.Services.AddSignalR();

// Add Authentication
builder.Services.AddJwtAuthenticationConfigurationAPI(builder.Configuration);

// Redis
builder.Services.AddRedisInfrastructure(builder.Configuration);

// Add Infrastructure Layer
builder.Services.AddServicesInfrastructure();

// Add Middleware
builder.Services.AddMiddlewareConfigurationAPI();

// Add MediatR
builder.Services.AddMediatRConfigurationApplication();

// Add Interceptor Persistence
builder.Services.AddInterceptorConfigurationPersistence();

// Add Connection SQL
//builder.Services.AddSqlServerRetryOptionsConfigurationPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
//builder.Services.AddSqlConfigurationPersistence();

// Add Connection POSTGRES
builder.Services.AddPostgresServerRetryOptionsConfigurationPersistence(
    builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddPostgresConfigurationPersistence();

// Add Repository Base
builder.Services.AddRepositoryBaseConfigurationPersistence();

// Add Dapper
//builder.Services.AddInfrastructureDapper();

// Add Swagger and Validation
builder.Services
    .AddSwaggerGenNewtonsoftSupport()
    .AddFluentValidationRulesToSwagger()
    .AddEndpointsApiExplorer()
    .AddSwaggerAPI();

// Add Swagger Api Versioning
builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Add Carter - Versioning Minimal API
builder.Services.AddCarter();

// Add Auto Mapper
builder.Services.AddAutoMapperConfigurationApplication();

// Add Authentication
//builder.Services.AddCookieAuthenticationConfigurationAPI();
// Configuration Seeder
// await builder.Services.AddInitialiserConfigurationPersistence(builder.Services.BuildServiceProvider());

// Configure App and Build
WebApplication app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Map Carter
app.MapCarter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.AddMigrations();

    // Seed data
    using (IServiceScope scope = app.Services.CreateScope())
    {
        IDbSeeder databaseSeeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
        await databaseSeeder.SeedAsync();
    }

    app.AddSwaggerConfigurationAPI();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();

app.UseAuthentication(); // This need to be added before app.UseAuthorization()
app.UseAuthorization();

app.MapControllers();

// // Initialize and seed the database
// await app.AddInitialiserConfigurationPersistence();

// Add Map Hub
app.MapHub<BookingHubBase>(HubsPath.BookingUrl);
app.MapHub<ChatHubBase>(HubsPath.ChatUrl);

try
{
    await app.RunAsync();
    Log.Information("Stopped cleanly");
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
    await app.StopAsync();
}
finally
{
    Log.CloseAndFlush();
    await app.DisposeAsync();
}

public partial class Program
{
}
