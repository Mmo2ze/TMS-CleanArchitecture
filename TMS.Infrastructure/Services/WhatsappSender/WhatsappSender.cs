using ErrorOr;
using MapsterMapper;
using Newtonsoft.Json;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Infrastructure.Services.WhatsappSender.ApiDefinition;
using TMS.Infrastructure.Services.WhatsappSender.ApiDefinition.Requests;
using TMS.Infrastructure.Services.WhatsappSender.ApiDefinition.Responses;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TMS.Infrastructure.Services.WhatsappSender;

public class WhatsappSender : IWhatsappSender
{
	private readonly IWhatsappApi _whatsappApi;
	private readonly IMapper _mapper;

	public WhatsappSender(IWhatsappApi whatsappApi, IMapper mapper)
	{
		_whatsappApi = whatsappApi;
		_mapper = mapper;
	}

	public bool IsValidNumber(string number)
	{
		throw new NotImplementedException();
	}

	public async Task<ErrorOr<string>> SendMessage(string number, string message)
	{
		var request = SendMessageRequest.SendPhoneMessage(number, message);
		var responseJson = await _whatsappApi.SendCode(request);
		const string invalidNumber = "This number doesn't have a WhatsApp account associated to it.";
		var successResponse = JsonSerializer.Deserialize<SendMessageSucces>(responseJson);
		if (successResponse is not null && successResponse.Sent == "true") return successResponse.MessageId;
		var errorResponse = JsonConvert.DeserializeObject<SendMessageError>(responseJson);
		if (errorResponse is not null && errorResponse.message == invalidNumber)
			return Errors.Whatsapp.WhatsappNotInstalled;
		return Errors.Whatsapp.WhatsappServiceFailed;

	}
}