using BadmintonSystem.API.DependencyInjection.Extensions;
using BadmintonSystem.API.Middleware;
using BadmintonSystem.Application.DependencyInjection.Extensions;
using BadmintonSystem.Infrastructure.Dapper.DependencyInjection.Extensions;
using BadmintonSystem.Infrastructure.DependencyInjection.Extensions;
using BadmintonSystem.Persistence.DependencyInjection.Extensions;
using BadmintonSystem.Persistence.DependencyInjection.Options;
using Carter;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
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

// Add Config Dapper
builder.Services.AddInfrastructureDapper();

// Add config Carter
builder.Services.AddCarter();

// Add config DI
builder.Services.AddInfrastructureServices();

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

#region ==================================== AddAuthentication with Cookies ================================
//// Add Authentication
//builder.Services.AddAuthentication(options =>
//{
//    // Có hoặc không
//    // AddCookie( "Cookies", options => {})
//    // If has from 2 AddCookies should use it
//    // AddCoookie( "Cookies_2", options => {})
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Default = " Cookies
//    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Default = " Cookies

//    // If use 2 AddCoookies
//    // If want use thì bỏ cmt cái đó
//    // options.DefaultAuthenticateScheme = "Cookies_2"; // Default = " Cookies
//    // options.DefaultChallengeScheme = "Cookies_2"; // Default = " Cookies
//}).AddCookie(options =>
//{
//    options.Cookie = new CookieBuilder()
//    {
//        // Setup for cookies Name, Domain,
//        // Domain == muốn lưu cookies ở đâu Ex: Google, ...
//        Name = "TestCookies",
//    };
//    options.LoginPath = "/api/v1/authen/unauthorized"; // If not has cookies then navigate to Link
//    options.LogoutPath = "/api/v1/authen/logout"; // If not has cookies then navigate to Link
//    options.AccessDeniedPath = "/api/v1/authen/forbidden"; // If not has cookies then navigate to Link

//    // Tạo ra lúc login == Principal
//    // Vào đây kiểm tra xem có những thông tin giống ở Principal lúc login hay không
//    // Có đung là User không
//    // ==> tiếp theo xuống "Cookies_2"
//    // ==================== Author thì sẽ khôgn sử dụng ở đây tạo ra class kế thừa
//    // Custom lại
//    // options.Events.OnValidatePrincipal = (context) =>
//    // {
//    //    return Task.CompletedTask;
//    // };
//})
////.AddCookie("Cookies_2", options =>
////{
////    options.Cookie = new CookieBuilder()
////    {
////        // Setup for cookies Name, Domain,
////        // Domain == muốn lưu cookies ở đâu Ex: Google, ...
////        Name = "TestCookies_V2",
////    };
////    options.LoginPath = "/api/authen/unauthorizedV2"; // If not has cookies then navigate to Link

////    // ==================== Author thì sẽ khôgn sử dụng ở đây tạo ra class kế thừa
////    // Custom lại
////    options.Events.OnValidatePrincipal = (context) =>
////    {
////        return Task.CompletedTask;
////    };
////})
//;

//// If not use
//// options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme and
//// options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme
//// Use Authorization ==> Auto run 2
////builder.Services.AddAuthorization(options =>
////{
////    var defaultAuthorizationPolicayBuilder = new AuthorizationPolicyBuilder(
////        CookieAuthenticationDefaults.AuthenticationScheme,
////        "Cookies_2");

////    defaultAuthorizationPolicayBuilder = defaultAuthorizationPolicayBuilder.RequireAuthenticatedUser();
////    options.DefaultPolicy = defaultAuthorizationPolicayBuilder.Build();
////});

#endregion ==================================== AddAuthentication with Cookies ================================

// Add Config Authentication with JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// Config http
builder.Services.AddHttpClient("OurWebApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5104/");
});

var app = builder.Build();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Add API Endpoint " Minimal API "
// "1"
// If using Carter then cmt, just using
//app.NewVersionedApi("Minimal-API-Gender").MapGenderApiV1().MapGenderApiV2();

// Add API With Carter
// Map all những cái nào kế thừa : ICarterModule
// Không cần phải use "1" của Minimal API
app.MapCarter();

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
