using MassTransit;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Consumer.MessageTemplates;
using TMS.Domain.Common.Repositories;
using TMS.MessagingContracts.Payments;

namespace TMS.Consumer.Consumers.Payments;

public class PaymentUpdatedEventHandler : IConsumer<PaymentUpdatedEvent>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IWhatsappSender _whatsappSender;
    private readonly ITeacherRepository _teacherRepository;

    public PaymentUpdatedEventHandler(IAccountRepository accountRepository, IWhatsappSender whatsappSender,
        ITeacherRepository teacherRepository)
    {
        _accountRepository = accountRepository;
        _whatsappSender = whatsappSender;
        _teacherRepository = teacherRepository;
    }

    public async Task Consume(ConsumeContext<PaymentUpdatedEvent> context)
    {
        var account = await _accountRepository.GetQueryable()
            .Include(x => x.Parent)
            .Include(x => x.Student)
            .Select(x => new
            {
                x.Id,
                parent = x.Parent != null
                    ? new
                    {
                        x.Parent.Name,
                        x.Parent.Phone
                    }
                    : null,
                student = new
                {
                    x.Student.ShortName,
                    x.Student.Gender
                }
            })
            .FirstAsync(x => x.Id == context.Message.AccountId);
        if (account.parent?.Phone is null)
            return;
        var message = MsgTemplate.Payment.PaymentUpdated(account.parent.Name, account.student.ShortName,
            context.Message.Amount, context.Message.PaymentDate, account.student.Gender);
        var teacher = await _teacherRepository.FindAsync(context.Message.TeacherId);
        await _whatsappSender.Send(account.parent.Phone, message, teacher);
    }
}