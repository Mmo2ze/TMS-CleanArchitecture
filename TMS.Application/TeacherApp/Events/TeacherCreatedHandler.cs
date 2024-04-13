using IntegrationEvents;
using MediatR;
using TMS.Application.Common.MessageTemplates;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;
using TMS.Domain.Teachers.Events;

namespace TMS.Application.TeacherApp.Events;

public class TeacherCreatedHandler : INotificationHandler<TeacherCreatedIntegrationEvent>
{
    private readonly IWhatsappSender _whatsappSender;

    public TeacherCreatedHandler(IWhatsappSender whatsappSender)
    {
        _whatsappSender = whatsappSender;
    }

    public async Task Handle(TeacherCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var message = MsgTemplate.Teacher.WelcomeTeacher(notification.TeacherName, notification.EndOfSubscription);
        await _whatsappSender.SendMessage(notification.TeacherPhone, message);
    }
}