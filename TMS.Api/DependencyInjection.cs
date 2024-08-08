using System.Reflection;
using System.Text.Json.Serialization;
using Mapster;
using MapsterMapper;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace TMS.Api;

public static class DependencyInjection
{
	public static IServiceCollection AddApi(this IServiceCollection services,
		ConfigurationManager builderConfiguration)
	{
		services.AddControllers()
			.AddJsonOptions(options => //Enable enum string converter
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
			
			);
		services.AddSwagger();
		services.AddHttpContextAccessor();
		AddCross(services);
		AddMaster(services);

		return services;
	}

	private static void AddMaster(IServiceCollection services)
	{
		var config =  TypeAdapterConfig.GlobalSettings;
		config.Scan(Assembly.GetExecutingAssembly());
		services.AddSingleton(config);
		services.AddScoped<IMapper, ServiceMapper>();
		
	}


	private static void AddCross(IServiceCollection services)
	{
            services.AddCors(options =>
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(hostName => true);
                })
            );
	}



	private static void AddSwagger(this IServiceCollection services)
	{
		
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
		{
			options.AddSecurityDefinition(
				"oauth2",
				new OpenApiSecurityScheme
				{
					Description =
						"Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				}
			);
			options.OperationFilter<SecurityRequirementsOperationFilter>();
		});
	}
}