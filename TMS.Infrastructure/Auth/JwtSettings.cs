namespace TMS.Infrastructure.Auth;

public class JwtSettings
{
	public static string SectionName => "Jwt";
	public string Issuer { get; init; } = null!;
	public string Secret { get; init; } = null!;
	public string Audience { get; init; } = null!;
	public int ExpiryDays { get; set; }
	public int ExpireMinutes { get; set; }
}