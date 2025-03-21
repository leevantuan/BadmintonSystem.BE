using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static BadmintonSystem.Contract.Services.V1.Tenant.Command;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Tenant;

public sealed class CreateTenantCommandHandler(
    TenantDbContext context,
    IConfiguration configuration,
    IServiceProvider serviceProvider,
    IMapper mapper,
    UserManager<AppUser> userManager)
    : ICommandHandler<CreateTenantCommand>
{
    public async Task<Result> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        Guid tenantId = Guid.NewGuid();
        string codeTenant = StringExtension.GenerateCodeTenantFromRequest(DateTime.Now);
        string newConnectionString = null;
        if (request.Data.Isolated == true)
        {
            string dbName = $"BMTSYS_{codeTenant}";
            var defaultConnectionString = configuration.GetConnectionString("PostgresConnectionStrings");
            newConnectionString = defaultConnectionString.Replace("BMTSYS_DATABASE", dbName);

            try
            {
                using IServiceScope scope = serviceProvider.CreateScope();
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

        var tenant = mapper.Map<Domain.Entities.Tenant>(request.Data);
        tenant.Id = tenantId;
        tenant.Name = request.Data.Name;
        tenant.Code = codeTenant;
        tenant.Email = request.Data.Email;
        tenant.ConnectionString = newConnectionString;

        context.Tenants.Add(tenant);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(tenant);
    }
}
