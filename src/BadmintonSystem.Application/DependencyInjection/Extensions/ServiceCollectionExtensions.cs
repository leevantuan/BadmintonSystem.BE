using BadmintonSystem.Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonSystem.Application.DependencyInjection.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigureMediatR(this IServiceCollection services)
        => services.AddMediatR(cfg => // Add mediatR
        cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly)) // Register Assembly Application => AssemblyReference.Assembly in Application
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
        .AddValidatorsFromAssembly(Contract.AssemblyReference.Assembly, includeInternalTypes: true); // Add Assembly là đã add được rule cho nó , includeInternalTypes == true, Default = false
}
