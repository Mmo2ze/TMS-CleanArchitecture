using Refit;

namespace TMS.Infrastructure.Services.WhatsappSender.ApiDefinition.Requests;

public class BaseRequest
{
	[AliasAs("token")]
	public string Token { get; init; } = "cEULYWjA1xuVhbgTtmDSeA/K2CofpGb6TgrgUBHF+7oTWR039bdYC+oLrSi7UVbToUJGeKyZaXaeXRYiEkgFRw==";
}