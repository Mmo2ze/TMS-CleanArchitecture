using ErrorOr;
using MassTransit;
using TMS.Application.Common.Services;
using TMS.Consumer.MessageTemplates;
using TMS.Domain.Common.Errors;
using TMS.Domain.Teachers;
using TMS.Domain.Teachers.Events;
using TMS.MessagingContracts.Teacher;

namespace TMS.Consumer.Consumers;

public class TeacherSubscriptionAddedConsumer : IConsumer<SubscriptionAddedDomainEvent>
{
    private readonly IWhatsappSender _whatsappSender;

    public TeacherSubscriptionAddedConsumer(IWhatsappSender whatsappSender)
    {
        _whatsappSender = whatsappSender;
    }

    public async Task Consume(ConsumeContext<SubscriptionAddedDomainEvent> context)
    {
        var message =
            MsgTemplate.Teacher.SubscriptionUpdated(context.Message.Name, context.Message.EndOfSubscription);
        var messageId = await _whatsappSender.Send(context.Message.Phone, message);
        if (WhatsappServiceFailed(messageId))
        {
            throw new Exception(messageId.FirstError.Description);
        }
    }

    private static bool WhatsappServiceFailed(ErrorOr<string> messageId)
    {
        return messageId.IsError && messageId.FirstError == Errors.Whatsapp.WhatsappServiceFailed;
    }
}