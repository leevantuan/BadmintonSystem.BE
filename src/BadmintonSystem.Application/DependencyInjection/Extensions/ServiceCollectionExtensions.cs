using BadmintonSystem.Application.Behaviors;
using BadmintonSystem.Application.Mapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonSystem.Application.DependencyInjection.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigureMediatR(this IServiceCollection services)
        => services.AddMediatR(cfg => // Add mediatR
        cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly)) 
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
        // .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationDefaultBehavior<,>)) // Defaul
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>))
        .AddValidatorsFromAssembly(Contract.AssemblyReference.Assembly, includeInternalTypes: true); 

    public static IServiceCollection AddConfigurationAutoMapper(this IServiceCollection services)
        => services.AddAutoMapper(typeof(ServiceProfile)); // Add config AutoMapper
}
