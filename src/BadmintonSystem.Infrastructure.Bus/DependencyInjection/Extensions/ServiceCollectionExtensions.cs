using System.Reflection;
using BadmintonSystem.Infrastructure.Bus.Consumers.Commands;
using BadmintonSystem.Infrastructure.Bus.Consumers.Events;
using BadmintonSystem.Infrastructure.Bus.DependencyInjection.Options;
using BadmintonSystem.Infrastructure.Bus.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonSystem.Infrastructure.Bus.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMediatRInfrastructureBus(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly))
            .AddTransient(typeof(IBookingService), typeof(BookingService))
            .AddTransient(typeof(IEmailService), typeof(EmailService));
    }

    public static void AddMassTransitRabbitMqInfrastructureBus
        (this IServiceCollection services, IConfiguration configuration)
    {
        // Đọc cấu hình Redis từ appsettings.json
        string env = configuration["Environment"] ?? "Development";
        _ = env.Equals("Development", StringComparison.OrdinalIgnoreCase);

        var masstransitConfiguration = new MasstransitConfiguration();

        configuration.GetSection(nameof(MasstransitConfiguration)).Bind(masstransitConfiguration);

        services.AddMassTransit(mt =>
        {
            // Add consumer Assembly vào Masstransit
            mt.AddConsumers(Assembly.GetExecutingAssembly());

            // Config rabbit MQ
            mt.UsingRabbitMq((context, bus) =>
            {
                // Config Exchange
                bus.Host(masstransitConfiguration.Host,
                    masstransitConfiguration.VHost, h =>
                    {
                        h.Username(masstransitConfiguration.UserName);
                        h.Password(masstransitConfiguration.Password);
                    });

                ConfigureReceiveEndpoint<EmailNotificationBusEventConsumer>(bus, context, "send-mail-event-queue");
                ConfigureReceiveEndpoint<SendEmailBusCommandConsumer>(bus, context, "send-mail-client-queue");
                ConfigureReceiveEndpoint<SendEmailBusCommandConsumer>(bus, context, "send-mail-staff-queue");
                ConfigureReceiveEndpoint<SendUpdateCacheBusCommandConsumer>(bus, context, "send-update-cache-queue");
                ConfigureReceiveEndpoint<SendCreateBookingCommandConsumer>(bus, context, "send-create-booking-queue");

                bus.ConfigureEndpoints(context);
            });
        });
    }

    private static void ConfigureReceiveEndpoint<TConsumer>
        (IBusFactoryConfigurator bus, IRegistrationContext context, string queueName)
        where TConsumer : class, IConsumer
    {
        bus.ReceiveEndpoint(queueName, e =>
        {
            e.PrefetchCount = 1; // Chỉ nhận 1 message tại một thời điểm
            e.ConcurrentMessageLimit = 1; // Chỉ xử lý 1 message tại một thời điểm
            e.ConfigureConsumer<TConsumer>(context);
        });
    }
}
