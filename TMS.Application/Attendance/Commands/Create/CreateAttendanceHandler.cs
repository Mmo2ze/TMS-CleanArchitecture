using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Attendance.Commands.Create;

public class CreateAttendanceHandler : IRequestHandler<CreateAttendanceCommand, ErrorOr<AttendanceResult>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAttendanceHandler(IAccountRepository accountRepository, ITeacherHelper teacherHelper, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _teacherHelper = teacherHelper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<AttendanceResult>> Handle(CreateAttendanceCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.FindAsync(request.AccountId, cancellationToken);
        var attendance = account!.AddAttendance(request.Status, request.Date, _teacherHelper.GetAssistantId());

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new AttendanceResult(
            attendance.Id,
            attendance.Date,
            attendance.Status,
            _teacherHelper.GetAssistantInfo(),
            null,
            null
        );
    }
}