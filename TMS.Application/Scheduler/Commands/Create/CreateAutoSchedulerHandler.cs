using ErrorOr;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.AttendanceSchedulers.Enums;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;

namespace TMS.Application.AttendanceScheduler.Commands.Create;

public class CreateAutoSchedulerHandler : IRequestHandler<CreateAutoSchedulerCommand,
    ErrorOr<List<Domain.AttendanceSchedulers.Scheduler>>>
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

    public async Task<ErrorOr<List<Domain.AttendanceSchedulers.Scheduler>>> Handle(
        CreateAutoSchedulerCommand request, CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        var teacher = _teacherRepository.GetQueryable()
            .Include(x => x.AttendanceSchedulers)
            .First(x => x.Id == teacherId);
        teacher.RemoveSchedulers();
        List<Domain.AttendanceSchedulers.Scheduler> newSchedulers = [];
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
                    Domain.AttendanceSchedulers.Scheduler.Create(session.Day, session.EndTime, teacherId));
            }
        }
        else if (request.SchedulerOption == AutoAttendanceSchedulerOption.AfterLastSessionOfSameGrade)
        {
            var sessionsGroupedByGrade = sessions.GroupBy(x => x.Grade);
            foreach (var group in sessionsGroupedByGrade)
            {
                var lastSession = group.Last();
                newSchedulers.Add(
                    Domain.AttendanceSchedulers.Scheduler.Create(lastSession.Day, lastSession.EndTime,
                        teacherId, lastSession.Grade));
            }
        }

        teacher.AddSchedulers(newSchedulers);
        return newSchedulers;
    }
}