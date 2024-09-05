using BadmintonSystem.API.DependencyInjection.Extensions;
using BadmintonSystem.API.Middleware;
using BadmintonSystem.Application.DependencyInjection.Extensions;
using BadmintonSystem.Persistence.DependencyInjection.Extensions;
using BadmintonSystem.Persistence.DependencyInjection.Options;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure Serilog.AspNet
Log.Logger = new LoggerConfiguration().ReadFrom
    .Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging
    .ClearProviders()
    .AddSerilog();

builder.Host.UseSerilog();

// Add Configure MediatR
builder.Services.AddConfigureMediatR();

// Add Config API
builder.Services
        .AddControllers()
        .AddApplicationPart(BadmintonSystem.Presentation.AssemblyReference.Assembly);

// Add Configure MiddleWare
builder.Services.AddConfigureMiddleware();

// Configurations để trước builder.Build()
// Add Config DATABASE SQLSERVER ==>
builder.Services.ConfigureSqlServerRetryOptions(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlConfiguration();

// Add Configure RepositoryBase
builder.Services.AddRepositoryBaseConfiguration();

// Add Configure AutoMapper
builder.Services.AddConfigurationAutoMapper();

// Add SWagger
builder.Services
        .AddSwaggerGenNewtonsoftSupport()
        .AddFluentValidationRulesToSwagger()
        .AddEndpointsApiExplorer()
        .AddSwagger();

builder.Services.AddControllers();

// Add config Swagger API Versioning
builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Default = " Cookies
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Default = " Cookies
}).AddCookie(options =>
{
    options.LoginPath = "/api/authen/unauthorized"; // If not has cookies then navigate to Link
});

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// Authentication ==> Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Run Version
// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
    app.ConfigureSwagger();

// Using Log of Serilog
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
{ }
