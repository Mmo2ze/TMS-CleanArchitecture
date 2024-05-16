using Refit;

namespace TMS.Infrastructure.Services.WhatsappSender.ApiDefinition.Requests;

public class BaseRequest
{
	[AliasAs("token")]
	public string Token { get; init; } = "JIRce4dfFw78JTcBqonJMCGvD2uNMWDb7UC0KIFu42w2Ltkg/6nvwWIwE/vJlqbC";
}