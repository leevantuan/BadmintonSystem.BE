using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static BadmintonSystem.Contract.Services.V1.Tenant.Command;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Tenant;

public sealed class CreateTenantCommandHandler : ICommandHandler<CreateTenantCommand>
{
    private readonly TenantDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public CreateTenantCommandHandler(TenantDbContext context, IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _context = context;
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    public async Task<Result> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        Guid tenantId = Guid.NewGuid();
        string newConnectionString = null;
        if (request.Data.Isolated == true)
        {
            string dbName = $"BMTSYS_DATABASE_Tenant_{request.Data.Name}";
            var defaultConnectionString = _configuration.GetConnectionString("PostgresConnectionStrings");
            newConnectionString = defaultConnectionString.Replace("BMTSYS_DATABASE", dbName);

            try
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.SetConnectionString(newConnectionString);
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating database {dbName}", ex);
            }
        }

        Domain.Entities.Tenant tenant = new Domain.Entities.Tenant
        {
            Id = tenantId,
            Name = request.Data.Name,
            Email = request.Data.Email,
            ConnectionString = newConnectionString
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
