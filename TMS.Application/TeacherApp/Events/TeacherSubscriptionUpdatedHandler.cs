using MediatR;
using TMS.Application.Common.MessageTemplates;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Teachers.Events;

namespace TMS.Application.TeacherApp.Events;

public class TeacherSubscriptionUpdatedHandler:INotificationHandler<TeacherSubscriptionUpdateIntegrationEvent>
{
    private readonly IWhatsappSender _whatsappSender;

    public TeacherSubscriptionUpdatedHandler(IWhatsappSender whatsappSender)
    {
        _whatsappSender = whatsappSender;
    }

    public async Task Handle(TeacherSubscriptionUpdateIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var message = MsgTemplate.Teacher.SubscriptionUpdated(notification.Name, notification.NewEOfSubscription);
        var result = await _whatsappSender.SendMessage(notification.TeacherPhone, message);
        if (result.IsError && result.FirstError == Errors.Whatsapp.WhatsappServiceFailed)
        {
            throw new Exception("Whatsapp service failed");
            // log error
        }
    }
}