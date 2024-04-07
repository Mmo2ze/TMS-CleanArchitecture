using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMS.Infrastructure.Persistence;


namespace TMS.Infrastructure;

public static class DependencyInjection
{
	public static void AddInfrastructure(this IServiceCollection services,
		ConfigurationManager builderConfiguration)
	{
		services.AddPersistence(builderConfiguration);
	}

	private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
	{
        ArgumentNullException.ThrowIfNull(configuration);
        var databaseSettings = configuration.GetSection(MainContextSettings.SectionName).Get<MainContextSettings>()!;
		services.AddDbContext<MainContext>(options =>
		{
			options.UseMySql(
					connectionString:
					$@"Server={databaseSettings.Server};port=3306;User ID={databaseSettings.Username};
						database={databaseSettings.DataBaseName};Password='{databaseSettings.Password}';",
					new MySqlServerVersion(new Version(8, 0, 25)))
				.EnableSensitiveDataLogging()
				.EnableDetailedErrors();
		});
	}
}