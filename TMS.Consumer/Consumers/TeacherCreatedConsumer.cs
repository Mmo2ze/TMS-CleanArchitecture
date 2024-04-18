using MassTransit;
using TMS.Application.Common.Services;
using TMS.Infrastructure.Services.WhatsappSender.ApiDefinition;
using TMS.MessagingContracts;

namespace TMS.Consumer.Consumers;

public class TeacherCreatedConsumer : IConsumer<TeacherCreatedEvent>
{
    private readonly IWhatsappSender _whatsappSender;
    private static int _counter = 0;

    public TeacherCreatedConsumer(IWhatsappSender whatsappSender)
    {
        _whatsappSender = whatsappSender;
    }

    public Task Consume(ConsumeContext<TeacherCreatedEvent> context)
    {
        var message = context.Message;
        _whatsappSender.SendMessage(message.Phone, "Welcome to our school!");
        Console.WriteLine($"Teacher created: {message.Name} with phone: {message.Phone}");
        return Task.CompletedTask;

    }
}