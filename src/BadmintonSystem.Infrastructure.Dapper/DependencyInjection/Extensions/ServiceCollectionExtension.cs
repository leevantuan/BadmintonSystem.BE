using BadmintonSystem.Domain.Abstractions.Dappers;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.AdditionalService;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Category;
using BadmintonSystem.Domain.Abstractions.Dappers.Repositoies.Gender;
using BadmintonSystem.Infrastructure.Dapper.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonSystem.Infrastructure.Dapper.DependencyInjection.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructureDapper(this IServiceCollection services)
        => services.AddTransient<IGenderRepository, GenderRepository>()
                   .AddTransient<ICategoryRepository, CategoryRepository>()
                   .AddTransient<IAdditionalServiceRepository, AdditionalServiceRepository>()
                   .AddTransient<IUnitOfWork, UnitOfWork>();
}
