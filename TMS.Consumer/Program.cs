// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Mapster;
using MapsterMapper;
using MassTransit;
using Microsoft.Extensions.Options;
using Refit;
using TMS.Application.Common.Services;
using TMS.Infrastructure.Services.WhatsappSender;
using TMS.Infrastructure.Services.WhatsappSender.ApiDefinition;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IWhatsappSender, WhatsappSender>();

builder.Services.AddOptions<WhatsappSenderSettings>()
    .BindConfiguration(WhatsappSenderSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();
var config =  TypeAdapterConfig.GlobalSettings;
config.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();
builder.Services.AddScoped<IWhatsappSender, WhatsappSender>();
builder.Services.AddRefitClient<IWhatsappApi>()
    .ConfigureHttpClient((sp, client) =>
        {
            client.BaseAddress = new Uri("https://whatsapp-messaging-hub.p.rapidapi.com/");
            client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "655a626a14msheb54a4a1d53f838p13cd59jsnae75c0fcca70");
            client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "whatsapp-messaging-hub.p.rapidapi.com");
        }
    );

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    var assembly = typeof(Program).Assembly;
    x.AddConsumers(assembly);
    x.AddSagaStateMachines(assembly);
    x.AddSagas(assembly);
    x.AddActivities(assembly);


    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.Run();