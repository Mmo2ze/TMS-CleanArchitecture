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
                    h.AccessKey("AKIA2UC264DEPT4T5FM6");
                    h.SecretKey("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");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}