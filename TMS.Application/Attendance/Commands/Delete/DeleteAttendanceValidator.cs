using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Attendances;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Attendance.Commands.Delete;

public class DeleteAttendanceValidator : AbstractValidator<DeleteAttendanceCommand>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAttendanceRepository _attendanceRepository;

    public DeleteAttendanceValidator(ITeacherHelper teacherHelper, IAttendanceRepository attendanceRepository)
    {
        _teacherHelper = teacherHelper;
        _attendanceRepository = attendanceRepository;
        RuleFor(x => x.Id)
            .MustAsync(BeFound)
            .WithError(Errors.Attendance.NotFound);
    }

    private Task<bool> BeFound(AttendanceId arg1, CancellationToken arg2)
    {
        return _attendanceRepository.AnyAsync(x => x.Id == arg1 && x.TeacherId == _teacherHelper.GetTeacherId(), arg2);
    }
}