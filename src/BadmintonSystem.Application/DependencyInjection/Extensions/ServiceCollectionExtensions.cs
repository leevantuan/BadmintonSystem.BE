using BadmintonSystem.Application.Behaviors;
using BadmintonSystem.Application.Mappers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonSystem.Application.DependencyInjection.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRConfigurationApplication(this IServiceCollection services)
        => services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly))
                   .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
                   .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformancePipelineBehavior<,>))
                   .AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>))
                   .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));
    //.AddValidatorsFromAssembly(BadmintonSystem.Contract.AssemblyReference.Assembly, includeInternalTypes: true);

    public static IServiceCollection AddAutoMapperConfigurationApplication(this IServiceCollection services)
        => services.AddAutoMapper(typeof(ServiceV1Profile))
                   .AddAutoMapper(typeof(ServiceV2Profile))
                   .AddAutoMapper(typeof(ServiceV3Profile));
}
