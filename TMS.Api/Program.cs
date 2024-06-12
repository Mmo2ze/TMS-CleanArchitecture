using MassTransit;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using TMS.Api;
using TMS.Application;
using TMS.Infrastructure;
using TMS.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\temp-keys\"))
    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
    {
        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
        ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
    });
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration);
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.AddEntityFrameworkOutbox<MainContext>(o =>
    {
        o.UsePostgres();
        // enable the bus outbox
        o.QueryDelay = TimeSpan.FromSeconds(5);
        o.UseBusOutbox();
    });
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumers(typeof(Program).Assembly);
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        // Use the CloudAMQP hostname and credentials with virtual host
        configurator.Host(new Uri("rabbitmq://crow.rmq.cloudamqp.com/fpccinpw"), h =>
        {
            h.Username("fpccinpw");
            h.Password("rl26jiMiiLoIpgMtif8XStEthJWlWLpb");
        });

        // Additional configuration if needed, e.g., retry policy, etc.
        // configurator.UseRetry(retryConfig => retryConfig.Interval(3, TimeSpan.FromSeconds(5)));
    });
});
var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true")); //save token
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();