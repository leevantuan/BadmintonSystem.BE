﻿using BadmintonSystem.API.DependencyInjection.Extensions;
using BadmintonSystem.API.Middleware;
using BadmintonSystem.Application.DependencyInjection.Extensions;
using BadmintonSystem.Persistence.DependencyInjection.Extensions;
using BadmintonSystem.Persistence.DependencyInjection.Options;
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

// Add MediatR
builder.Services.AddConfigureMediatR();
builder.Services.AddConfigureMiddleware();

// Add Config API
builder
    .Services
    .AddControllers()
    .AddApplicationPart(BadmintonSystem.Presentation.AssemblyReference.Assembly);

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

// Add Configuration DependencyInjection
// Configurations để trước builder.Build()
// Add Config DATABASE SQLSERVER ==>
builder.Services.AddRepositoryBaseConfiguration();
builder.Services.ConfigureSqlServerRetryOptions(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlConfiguration();
builder.Services.AddConfigurationAutoMapper();

// Add Authentication
builder.Services.AddAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.ConfigureSwagger();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication ==> Authorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.Run();
// Run Version
//if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
//    app.ConfigureSwagger();

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

public partial class Program { }

