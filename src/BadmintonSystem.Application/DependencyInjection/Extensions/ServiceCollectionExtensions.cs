using BadmintonSystem.Application.Behaviors;
using BadmintonSystem.Application.Mappers;
using BadmintonSystem.Application.UseCases.V1.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonSystem.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatRConfigurationApplication(this IServiceCollection services)
    {
        return services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformancePipelineBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>))
            .AddTransient(typeof(IYardPriceService), typeof(YardPriceService))
            .AddTransient(typeof(IBookingLineService), typeof(BookingLineService))
            .AddTransient(typeof(IBillService), typeof(BillService))
            .AddTransient(typeof(IBillLineService), typeof(BillLineService))
            .AddTransient(typeof(IServiceLineService), typeof(ServiceLineService))
            .AddTransient(typeof(IOriginalQuantityService), typeof(OriginalQuantityService))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>))
            .AddValidatorsFromAssembly(Contract.AssemblyReference.Assembly, includeInternalTypes: true);
    }
    //.AddValidatorsFromAssembly(BadmintonSystem.Contract.AssemblyReference.Assembly, includeInternalTypes: true);

    public static IServiceCollection AddAutoMapperConfigurationApplication(this IServiceCollection services)
    {
        return services.AddAutoMapper(typeof(ServiceV1Profile))
            .AddAutoMapper(typeof(ServiceV2Profile))
            .AddAutoMapper(typeof(ServiceV3Profile));
    }
}
