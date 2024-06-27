using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Schedulers.Enums;

namespace TMS.Application.Scheduler.Commands.Create;

public class CreateAutoSchedulerHandler : IRequestHandler<CreateAutoSchedulerCommand,
    ErrorOr<List<Domain.Schedulers.Scheduler>>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly ITeacherRepository _teacherRepository;
    private readonly ISessionRepository _sessionRepository;

    public CreateAutoSchedulerHandler(ITeacherHelper teacherHelper,
        ISchedulerRepository schedulerRepository, ITeacherRepository teacherRepository,
        ISessionRepository sessionRepository)
    {
        _teacherHelper = teacherHelper;
        _teacherRepository = teacherRepository;
        _sessionRepository = sessionRepository;
    }

    public async Task<ErrorOr<List<Domain.Schedulers.Scheduler>>> Handle(
        CreateAutoSchedulerCommand request, CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        var teacher = _teacherRepository.GetQueryable()
            .Include(x => x.AttendanceSchedulers)
            .First(x => x.Id == teacherId);
        teacher.RemoveSchedulers();
        List<Domain.Schedulers.Scheduler> newSchedulers = [];
        var sessions = _sessionRepository.WhereQueryable(x => x.TeacherId == teacherId).Select(x =>
            new
            {
                x.Day,
                x.EndTime,
                x.Grade
            }
        );
        if (request.SchedulerOption == AutoAttendanceSchedulerOption.AfterEverySession)
        {
            foreach (var session in sessions)
            {
                newSchedulers.Add(
                    Domain.Schedulers.Scheduler.Create(session.Day, session.EndTime, teacherId));
            }
        }
        else if (request.SchedulerOption == AutoAttendanceSchedulerOption.AfterLastSessionOfSameGrade)
        {
            var sessionsGroupedByGrade = sessions.GroupBy(x => x.Grade);
            foreach (var group in sessionsGroupedByGrade)
            {
                var sessionsByDay = group.GroupBy(x => x.Day);
                foreach (var day in sessionsByDay)
                {
                    var lastSession = day.OrderBy(x => x.EndTime).Last();
                    newSchedulers.Add(
                        Domain.Schedulers.Scheduler.Create(lastSession.Day, lastSession.EndTime,
                            teacherId, lastSession.Grade));
                }

            }
        }

        teacher.AddSchedulers(newSchedulers);
        return newSchedulers;
    }
}