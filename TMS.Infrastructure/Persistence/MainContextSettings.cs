namespace TMS.Infrastructure.Persistence;

public class MainContextSettings
{
		public static string SectionName = "DateBase";
		public string ConnectionString { get; set; } = null!;
		public string Username { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string DataBaseName { get; set; } = null!;
		public string Server { get; set; } = null!;	
}