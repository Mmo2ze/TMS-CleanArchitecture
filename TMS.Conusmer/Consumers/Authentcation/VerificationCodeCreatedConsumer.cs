using MassTransit;
using TMS.Application.Common.Services;
using TMS.Conusmer.MessageTemplates;
using TMS.MessagingContracts.Authentication;

namespace TMS.Conusmer.Consumers.Authentcation;

public class VerificationCodeCreatedConsumer : IConsumer<VerificationCodeCreatedEvent>
{
    private readonly IWhatsappSender _whatsappSender;

    public VerificationCodeCreatedConsumer(IWhatsappSender whatsappSender)
    {
        _whatsappSender = whatsappSender;
    }


    public async Task Consume(ConsumeContext<VerificationCodeCreatedEvent> context)
    {

            var message = MsgTemplate.Auth.VerificationCodeCreated(context.Message.Code);
            var messageId = await _whatsappSender.Send(context.Message.Phone, message);
    }

    
}