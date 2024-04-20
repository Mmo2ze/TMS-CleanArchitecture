using MassTransit;
using TMS.Application.Common.Services;
using TMS.Consumer.MessageTemplates;
using TMS.MessagingContracts.Teacher;

namespace TMS.Consumer.Consumers;

public class TeacherCreatedConsumer : IConsumer<TeacherCreatedEvent>
{
    private readonly IWhatsappSender _whatsappSender;

    public TeacherCreatedConsumer(IWhatsappSender whatsappSender)
    {
        _whatsappSender = whatsappSender;
    }

    public async Task Consume(ConsumeContext<TeacherCreatedEvent> context)
    {
        var message = context.Message;
        string? whatsappMessage;
        string? whatsappPhone;
        if (await NotHasWhatsapp(message))
        {
            whatsappMessage = MsgTemplate.Admin.TeacherPhoneNotHaveWhatsapp(message.Name, message.TeacherPhone);
            whatsappPhone = message.CreatedByPhone;
        }
        else
        {
            whatsappMessage = MsgTemplate.Teacher.WelcomeTeacher(message.Name, message.EOfSubscription);
            whatsappPhone = message.TeacherPhone;
        }

        var messageID = await _whatsappSender.SendMessage(whatsappPhone,whatsappMessage );
    }

    private async Task<bool> NotHasWhatsapp(TeacherCreatedEvent message)
    {
        return !await _whatsappSender.IsValidNumber(message.TeacherPhone);
    }
}