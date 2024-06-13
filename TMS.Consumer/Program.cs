// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Mapster;
using MapsterMapper;
using MassTransit;
using TMS.Domain.Common.Repositories;
using TMS.Infrastructure;
using TMS.Infrastructure.Persistence;
using TMS.Infrastructure.Persistence.Repositories;
using TMS.Infrastructure.Services.WhatsappSender.ApiDefinition;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddMediatR(options => { options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly); });
builder.Services.AddOptions<WhatsappSenderSettings>()
    .BindConfiguration(WhatsappSenderSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.Configure<RabbitMqTransportOptions>(builder.Configuration.GetSection(MainContextSettings.SectionName));

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    var assembly = typeof(Program).Assembly;
    x.AddConsumers(assembly);
    x.AddSagaStateMachines(assembly);
    x.AddSagas(assembly);
    x.AddActivities(assembly);

    x.AddEntityFrameworkOutbox<MainContext>(o =>
    {
        o.DuplicateDetectionWindow = TimeSpan.FromSeconds(30);
        o.UseMySql();
        o.QueryDelay = TimeSpan.FromSeconds(1);
    });
    builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();

    x.UsingRabbitMq((context, cfg) =>
    {
        //cfg.PrefetchCount = 1;
        cfg.Host(new Uri("rabbitmq://crow.rmq.cloudamqp.com/fpccinpw"), h =>
        {
            h.Username("fpccinpw");
            h.Password("rl26jiMiiLoIpgMtif8XStEthJWlWLpb");
        });
        cfg.UseMessageRetry(r => r.Intervals(5000, 5200, 5500, 5800, 10000));

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.Run();