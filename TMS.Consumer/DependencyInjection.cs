using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMS.Infrastructure.Persistence;

namespace TMS.Consumer;

public static class DependencyInjection
{
    public static void AddConsumer(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddEntityFrameworkOutbox<MainContext>(o =>
            {
                o.UsePostgres();
                // enable the bus outbox
                o.QueryDelay = TimeSpan.FromSeconds(5);
                o.UseBusOutbox();
            });
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.AddConsumers(typeof(DependencyInjection).Assembly);
            busConfigurator.UsingAmazonSqs((context, cfg) =>
            {
                cfg.Host("eu-north-1", h =>
                {
                    h.AccessKey("AKIA2UC264DEHQPQI6SU");
                    h.SecretKey("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");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}