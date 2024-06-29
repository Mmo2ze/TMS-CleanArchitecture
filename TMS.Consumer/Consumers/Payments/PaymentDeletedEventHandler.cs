using MassTransit;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Consumer.MessageTemplates;
using TMS.Domain.Common.Repositories;
using TMS.MessagingContracts.Payments;

namespace TMS.Consumer.Consumers.Payments;

public class PaymentDeletedEventHandler : IConsumer<PaymentDeletedEvent>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IWhatsappSender _whatsappSender;

    public PaymentDeletedEventHandler(IAccountRepository accountRepository, ITeacherRepository teacherRepository,
        IWhatsappSender whatsappSender)
    {
        _accountRepository = accountRepository;
        _teacherRepository = teacherRepository;
        _whatsappSender = whatsappSender;
    }

    public async Task Consume(ConsumeContext<PaymentDeletedEvent> context)
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
        var message = MsgTemplate.Payment.PaymentRemoved(account.parent.Name, account.student.ShortName,
            context.Message.Amount, context.Message.BillDate, account.student.Gender);
        var teacher = await _teacherRepository.FindAsync(context.Message.TeacherId);
        await _whatsappSender.Send(account.parent.Phone, message, teacher);
    }
}