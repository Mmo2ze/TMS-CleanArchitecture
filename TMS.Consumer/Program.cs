// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Mapster;
using MapsterMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Refit;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Infrastructure.Persistence;
using TMS.Infrastructure.Persistence.Repositories;
using TMS.Infrastructure.Services.WhatsappSender;
using TMS.Infrastructure.Services.WhatsappSender.ApiDefinition;
using MainContext = TMS.Consumer.MainContext;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<IWhatsappSender, WhatsappSender>();

builder.Services.AddOptions<WhatsappSenderSettings>()
    .BindConfiguration(WhatsappSenderSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
var config = TypeAdapterConfig.GlobalSettings;
config.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddScoped<IWhatsappSender, WhatsappSender>();
builder.Services.Configure<WhatsappSenderSettings>(
    builder.Configuration.GetSection(WhatsappSenderSettings.SectionName));
builder.Services.AddRefitClient<IWhatsappApi>()
    .ConfigureHttpClient((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<WhatsappSenderSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", settings.RapidKey);
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", settings.RapidHost);
        }
    );
builder.Services.Configure<RabbitMqTransportOptions>(builder.Configuration.GetSection(MainContextSettings.SectionName));

builder.Services.AddDbContext<MainContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
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
    });
    builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.UseMessageRetry(r => r.Intervals(5000, 5200, 5500, 5800, 10000));

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.Run();