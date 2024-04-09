using Refit;
using TMS.Infrastructure.Services.WhatsappSender.ApiDefinition.Requests;
using TMS.Infrastructure.Services.WhatsappSender.ApiDefinition.Responses;

namespace TMS.Infrastructure.Services.WhatsappSender.ApiDefinition;

public interface IWhatsappApi
{
	[Get("/WhatsappGetMessages")]
	Task<GetMessagesResponse> GetMessages( GetMessagesParams param);

	[Post("/WhatsappSendMessage")]
	Task<string> SendCode([Body]SendMessageRequest request);

}






