using MassTransit;
using TMS.Api;
using TMS.Application;
using TMS.Infrastructure;
using TMS.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

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
        o.QueryDelay = TimeSpan.FromMilliseconds(2000);
        o.UseBusOutbox();
    });
	busConfigurator.SetKebabCaseEndpointNameFormatter();
	busConfigurator.AddConsumers(typeof(Program).Assembly);
	busConfigurator.UsingRabbitMq((context, configurator) =>
	{
	    configurator.Host("localhost", "/", h =>
	    {
	        h.Username("guest");
	        h.Password("guest");
	    });
	});
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c => c.ConfigObject.AdditionalItems.Add("persistAuthorization", "true"));		//save token
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();