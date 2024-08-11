using Coravel.Invocable;
using TMS.Application.Common.Services;
using TMS.Domain.Attendances;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Services.BackGroundJobs;

public class AttendanceCheckerJob : IInvocable
{
    private readonly IHolidayRepository _holidayRepository;
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AttendanceCheckerJob(ISchedulerRepository schedulerRepository,
        IHolidayRepository holidayRepository, IDateTimeProvider dateTimeProvider, IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _schedulerRepository = schedulerRepository;
        _holidayRepository = holidayRepository;
        _dateTimeProvider = dateTimeProvider;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Invoke()
    {
        var today = _dateTimeProvider.Today;
        var timeNow = _dateTimeProvider.TimeNow;
        var schedulers = _schedulerRepository
            .WhereQueryable(x =>
                x.Day == today.DayOfWeek &&
                x.FiresOn <= timeNow
            )
            .GroupBy(x => x.TeacherId)
            .ToList();
        var holidays = _holidayRepository
            .WhereQueryable(x => x.StartDate <= today && x.EndDate >= today)
            .ToList();

        foreach (var teacher in schedulers)
        {
            var accounts = _accountRepository.WhereQueryable(x =>
                    x.TeacherId == teacher.Key &&
                    // ReSharper disable once SimplifyLinqExpressionUseAll
                    !x.Attendances.Any(attendance => attendance.Date == today)
                )
                .ToList();
            var teacherSchedulers = teacher.ToList();
            foreach (var teacherScheduler in teacherSchedulers)
            {
                var holiday = holidays.SingleOrDefault(x => x.TeacherId == teacher.Key);
                if (holiday != null)
                {
                    if (holiday.GroupId != null)
                    {
                        var holidayAccounts = accounts.Where(x => x.GroupId == holiday.GroupId).ToList();
                        foreach (var holidayAccount in holidayAccounts)
                        {
                            holidayAccount.AddAttendance(AttendanceStatus.Holiday, today, today);
                        }

                        var nonHolidayAccounts = accounts.Where(x =>
                            x.GroupId != holiday.GroupId &&
                            (teacherScheduler.Grade == null || x.Grade == teacherScheduler.Grade)).ToList();
                        foreach (var nonHolidayAccount in nonHolidayAccounts)
                        {
                            nonHolidayAccount.AddAttendance(AttendanceStatus.Absent, today, today);
                        }

                        accounts.RemoveAll(x => nonHolidayAccounts.Contains(x));
                    }
                    else
                    {
                        foreach (var account in accounts)
                        {
                            account.AddAttendance(AttendanceStatus.Holiday, today, today);
                        }

                        accounts.Clear();
                    }
                }
                else
                {
                    var selectedStudents = accounts
                        .Where(x => teacherScheduler.Grade == null || x.Grade == teacherScheduler.Grade).ToList();

                    foreach (var account in selectedStudents)
                    {
                        account.AddAttendance(AttendanceStatus.Absent, today, today);
                    }
                }
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }
}