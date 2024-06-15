using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMS.Infrastructure.Persistence;


namespace TMS.Conusmer;

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
                cfg.Host("eu-west-3", h =>
                {
                    h.AccessKey("AKIA4IE4QFYDYIFRPWUP");
                    h.SecretKey("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");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}