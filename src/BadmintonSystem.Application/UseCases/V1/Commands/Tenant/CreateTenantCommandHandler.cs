using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services;
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
        string codeTenant = StringExtension.GenerateCodeTenantFromRequest(DateTime.Now);
        string newConnectionString = null;
        if (request.Data.Isolated == true)
        {
            string dbName = $"BMTSYS_{codeTenant}";
            var defaultConnectionString = _configuration.GetConnectionString("PostgresConnectionStrings");
            newConnectionString = defaultConnectionString.Replace("BMTSYS_DATABASE", dbName);

            try
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.SetConnectionString(newConnectionString);

                // Áp dụng migration cho tenant database
                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();

                    // Seed data cho tenant sau khi tạo database
                    IDbSeeder databaseSeeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
                    await databaseSeeder.SeedAsync();
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
            Code = codeTenant,
            Email = request.Data.Email,
            ConnectionString = newConnectionString
        };

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(tenant);
    }
}
