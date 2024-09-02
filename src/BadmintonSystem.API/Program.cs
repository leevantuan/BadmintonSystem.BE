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

// Add Config API
builder
    .Services
    .AddControllers()
    .AddApplicationPart(BadmintonSystem.Presentation.AssemblyReference.Assembly);

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Configuration DependencyInjection
// Configurations để trước builder.Build()
// Add Config DATABASE SQLSERVER ==>
builder.Services.AddRepositoryBaseConfiguration();
builder.Services.ConfigureSqlServerRetryOptions(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));
builder.Services.AddSqlConfiguration();
builder.Services.AddConfigurationAutoMapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
