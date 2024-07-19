using MassTransit;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Consumer.MessageTemplates;
using TMS.Domain.Common.Repositories;
using TMS.MessagingContracts.Attendances;

namespace TMS.Consumer.Consumers.Payments;

public class AbsenceRecordedEventHandler : IConsumer<AbsenceRecordedEvent>
{
    private readonly ITeacherRepository _teacherRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IWhatsappSender _whatsappSender;

    public AbsenceRecordedEventHandler(ITeacherRepository teacherRepository, IAccountRepository accountRepository,
        IWhatsappSender whatsappSender)
    {
        _teacherRepository = teacherRepository;
        _accountRepository = accountRepository;
        _whatsappSender = whatsappSender;
    }

    public async Task Consume(ConsumeContext<AbsenceRecordedEvent> context)
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
                    x.Student.Id,
                    x.Student.Gender,
                    x.Student.ShortName
                }
            })
            .FirstAsync(x => x.Id == context.Message.AccountId);
        if (account.parent?.Phone is null)
            return;

        var teacher = await _teacherRepository.FindAsync(context.Message.TeacherId);
        var message =
            MsgTemplate.Attendance.AbsenceRecorded(account.parent.Name, account.student.ShortName);
        // Send message to parent
        await _whatsappSender.Send(account.parent.Phone, message, teacher);
    }
}