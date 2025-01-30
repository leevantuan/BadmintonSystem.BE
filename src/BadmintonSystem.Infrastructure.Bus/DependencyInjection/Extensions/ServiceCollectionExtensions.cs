﻿using System.Reflection;
using BadmintonSystem.Infrastructure.Bus.Consumers.Events;
using BadmintonSystem.Infrastructure.Bus.DependencyInjection.Options;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BadmintonSystem.Infrastructure.Bus.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddMediatRInfrastructureBus(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly));
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
            // Add consumer từng cái
            // mt.AddConsumer<SendEmailWhenReceivedEmailEventConsumerEvent>();

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

                // Cấu hình ReceiveEndpoint với tên queue cụ thể
                bus.ReceiveEndpoint("send-mail-event", e =>
                {
                    e.ConfigureConsumer<EmailNotificationBusEventConsumer>(context);
                });

                // Config consumer
                bus.ConfigureEndpoints(context);
            });
        });
    }
}
