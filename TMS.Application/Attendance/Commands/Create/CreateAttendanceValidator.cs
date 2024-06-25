using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Attendance.Commands.Create;

public class CreateAttendanceValidator : AbstractValidator<CreateAttendanceCommand>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;
    private readonly IAttendanceRepository _attendanceRepository;

    public CreateAttendanceValidator(ITeacherHelper teacherHelper, IAccountRepository accountRepository,
        IAttendanceRepository attendanceRepository, IDateTimeProvider dateTimeProvider)
    {
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
        _attendanceRepository = attendanceRepository;


        RuleFor(x => x.Date).GreaterThanOrEqualTo(dateTimeProvider.Today);
        RuleFor(x => x.AccountId).MustAsync(BeFoundAccount)
            .WithError(Errors.Account.NotFound);

        RuleFor(x => x.Date).MustAsync(BeUniqueAttendance)
            .WithError(Errors.Attendance.AlreadyExist);
    }

    private async Task<bool> BeUniqueAttendance(CreateAttendanceCommand command ,DateOnly arg1, CancellationToken arg2)
    {
        return !await _attendanceRepository.AnyAsync(
            x => x.Date == arg1 &&
                 x.TeacherId == _teacherHelper.GetTeacherId() && 
                 x.AccountId == command.AccountId,
            arg2);
    }

    private Task<bool> BeFoundAccount(AccountId arg1, CancellationToken arg2)
    {
        return _accountRepository.AnyAsync(x => x.Id == arg1 && x.TeacherId == _teacherHelper.GetTeacherId(), arg2);
    }
}