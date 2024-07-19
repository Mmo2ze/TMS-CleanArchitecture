using ErrorOr;
using MapsterMapper;
using Newtonsoft.Json;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Teachers;
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

    public async Task<bool> IsValidNumber(string number)
    {
        var request = HasWhatsappRequest.Create(number);
        var response = await _whatsappApi.HasWhatsapp(request);
        return response.Status == "valid";
    }

    public async Task<ErrorOr<string>> Send(string number, string message, Teacher? teacher = null)
    {
        message = AddTeacherNameToMessage(message, teacher);
        var request = SendMessageRequest.SendPhoneMessage(number, message);
        var responseJson = await _whatsappApi.SendCode(request);
        const string invalidNumber = "Invalid WhatsApp number or Group ID";
        const string numberDontHaveWhatsApp = "This number doesn't have a WhatsApp account associated to it.";
        var successResponse = JsonSerializer.Deserialize<SendMessageSucces>(responseJson);
        if (successResponse is not null && successResponse.Sent == "true") return successResponse.MessageId;
        var errorResponse = JsonConvert.DeserializeObject<SendMessageError>(responseJson);
        if (errorResponse is not null && errorResponse.Message == numberDontHaveWhatsApp)
            return Errors.Whatsapp.WhatsappNotInstalled;
        if (errorResponse is not null && errorResponse.Message == invalidNumber)
            return Errors.Whatsapp.InvalidNumber;
        return Errors.Whatsapp.WhatsappServiceFailed;
    }

    private string AddTeacherNameToMessage(string message, Teacher? teacher)
    {
        if (teacher is null) return message;
        var suffix = $"مستر  {teacher.Name} مدرس {teacher.Subject.ToArabic()} " +
                     $"للتواصل معه عبر الواتساب اضغط على الرابط التالي: {teacher.WhatsappLink}";
        return message + suffix;
    }
}