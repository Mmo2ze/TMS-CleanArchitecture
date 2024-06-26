using MassTransit;
using TMS.Domain.AttendanceSchedulers;
using TMS.Domain.AttendanceSchedulers.Enums;
using TMS.Domain.Common.Repositories;
using TMS.MessagingContracts.Session;

namespace TMS.Consumer.Consumers.Sessions;

public class SessionCreatedEventHandler : IConsumer<SessionCreatedEvent>
{
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SessionCreatedEventHandler(ISchedulerRepository schedulerRepository, ITeacherRepository teacherRepository,
        IUnitOfWork unitOfWork)
    {
        _schedulerRepository = schedulerRepository;
        _teacherRepository = teacherRepository;
        _unitOfWork = unitOfWork;
    }

    public Task Consume(ConsumeContext<SessionCreatedEvent> context)
    {
        var message = context.Message;
        var schedulerOption = _teacherRepository.GetQueryable()
            .Select(x => new { x.Id, x.AttendanceScheduler })
            .First(x => x.Id == message.TeacherId).AttendanceScheduler;
        switch (schedulerOption)
        {
            case AutoAttendanceSchedulerOption.AfterEverySession:
            {
                var scheduler = Scheduler.Create(message.Day, message.EndTime, message.TeacherId);
                _schedulerRepository.Add(scheduler);
                break;
            }
            case AutoAttendanceSchedulerOption.AfterLastSessionOfSameGrade:
            {
                var oldScheduler = _schedulerRepository.WhereQueryable(x =>
                        x.TeacherId == message.TeacherId && x.Grade == message.Grade && x.Day == message.Day)
                    .FirstOrDefault();
                if (oldScheduler != null)
                {
                    if (oldScheduler.FiresOn < message.EndTime)
                    {
                        oldScheduler.UpdateFiresOn(message.EndTime);
                    }
                }
                else
                {
                    var scheduler = Scheduler.Create(message.Day, message.EndTime, message.TeacherId,
                        message.Grade);
                    _schedulerRepository.Add(scheduler);
                }


                break;
            }
        }

        _unitOfWork.SaveChangesAsync();
        return Task.CompletedTask;
    }

   
}