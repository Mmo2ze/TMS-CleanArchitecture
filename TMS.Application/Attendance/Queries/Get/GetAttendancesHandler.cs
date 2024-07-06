using ErrorOr;
using MediatR;
using TMS.Application.Attendance.Commands.Create;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Attendance.Queries.Get;

public class GetAttendancesHandler : IRequestHandler<GetAttendancesQuery, ErrorOr<GetAttendancesResult>>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly ITeacherHelper _teacherHelper;

    public GetAttendancesHandler(IAttendanceRepository attendanceRepository, ITeacherHelper teacherHelper)
    {
        _attendanceRepository = attendanceRepository;
        _teacherHelper = teacherHelper;
    }

    public Task<ErrorOr<GetAttendancesResult>> Handle(GetAttendancesQuery request,
        CancellationToken cancellationToken)
    {
        var teacherName = _teacherHelper.GetTeacherName();
        var attendances = _attendanceRepository.GetQueryable()
            .Where(x => x.AccountId == request.AccountId && x.Date.Year == request.Year && x.Date.Month == request.Month)
            .OrderByDescending(x => x.Date)
            .Select(x => new AttendanceResult(
                x.Id,
                x.Date,
                x.Status,
                new AssistantInfo(
                    x.CreatedBy != null ? x.CreatedBy.Name : teacherName,
                    x.CreatedById
                ),
                new AssistantInfo(
                    x.UpdatedBy != null ? x.UpdatedBy.Name : teacherName,
                    x.CreatedById
                ), x.UpdatedAt
            ));

        return Task.FromResult<ErrorOr<GetAttendancesResult>>(new GetAttendancesResult(attendances.ToList(), attendances.Count()));
    }
}