using ErrorOr;
using MediatR;
using TMS.Application.Attendance.Commands.Create;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Attendance.Queries.Get;

public class GetAttendancesHandler : IRequestHandler<GetAttendancesQuery, ErrorOr<PaginatedList<AttendanceResult>>>
{
    private readonly IAttendanceRepository _attendanceRepository;
    private readonly ITeacherHelper _teacherHelper;

    public GetAttendancesHandler(IAttendanceRepository attendanceRepository, ITeacherHelper teacherHelper)
    {
        _attendanceRepository = attendanceRepository;
        _teacherHelper = teacherHelper;
    }

    public async Task<ErrorOr<PaginatedList<AttendanceResult>>> Handle(GetAttendancesQuery request,
        CancellationToken cancellationToken)
    {
        var teacherName = _teacherHelper.GetTeacherName();
        var account = _attendanceRepository.GetQueryable()
            .Where(x => x.AccountId == request.AccountId)
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
        var response = await account.PaginatedListAsync(request.Page, request.PageSize);
        return response;
    }
}