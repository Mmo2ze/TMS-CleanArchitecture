using Refit;

namespace TMS.Infrastructure.Services.WhatsappSender.ApiDefinition.Requests;

public class BaseRequest
{
	[AliasAs("token")]
	public string Token { get; init; } = "+dyKeJf/L7BNWtDwociSWbxw58LgdpHAZK5G2Lv6rsrh5ca/Ea+yArQtoiolrBnIb3YYqqpaIX+X0m0wnIrFxQ==";
}