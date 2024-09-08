using BadmintonSystem.Domain.Abstractions.Dappers;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Gender;
using BadmintonSystem.Infrastructure.Dapper.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonSystem.Infrastructure.Dapper.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureDapper(this IServiceCollection services)
        => services.AddTransient<IGenderRepository, GenderRepository>()
                   .AddTransient<IUnitOfWork, UnitOfWork>();
}
