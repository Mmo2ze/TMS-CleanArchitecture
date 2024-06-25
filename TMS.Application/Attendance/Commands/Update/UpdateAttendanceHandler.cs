using ErrorOr;
using MediatR;
using TMS.Application.Attendance.Commands.Create;
using TMS.Application.Common.Services;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Attendance.Commands.Update;

public class UpdateAttendanceHandler : IRequestHandler<UpdateAttendanceCommand, ErrorOr<AttendanceResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAttendanceRepository _attendanceRepository;
    public UpdateAttendanceHandler(ITeacherHelper teacherHelper, IAttendanceRepository attendanceRepository)
    {
        _teacherHelper = teacherHelper;
        _attendanceRepository = attendanceRepository;
    }

    public async Task<ErrorOr<AttendanceResult>> Handle(UpdateAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var teacherName = _teacherHelper.GetTeacherName();

        var attendance = await _attendanceRepository.FindAsync(request.Id, cancellationToken);
        attendance!.Update(_teacherHelper.GetAssistantId(), request.Status);

        return new AttendanceResult(
            attendance.Id,
            attendance.Date,
            attendance.Status,
            new AssistantInfo(
                attendance.CreatedBy != null ? attendance.CreatedBy.Name : teacherName,
                attendance.CreatedById
            ),
            new AssistantInfo(
                attendance.UpdatedBy != null ? attendance.UpdatedBy.Name : teacherName,
                attendance.CreatedById
            ),
            attendance.UpdatedAt);
    }
}