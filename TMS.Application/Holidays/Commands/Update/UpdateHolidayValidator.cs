using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Holidays.Commands.Update;

public class UpdateHolidayValidator:AbstractValidator<UpdateHolidayCommand>
{
    private readonly IHolidayRepository _holidayRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateHolidayValidator(ITeacherHelper teacherHelper, IHolidayRepository holidayRepository, IGroupRepository groupRepository, IDateTimeProvider dateTimeProvider)
    {
        var teacherId = teacherHelper.GetTeacherId();
        _holidayRepository = holidayRepository;
        _groupRepository = groupRepository;
        _dateTimeProvider = dateTimeProvider;
        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(dateTimeProvider.Today)
            .WithMessage($"Start date must be greater than or equal to today {dateTimeProvider.Today}");

        RuleFor(x => x.EndDate)
            .Must((command, endDate) => endDate >= command.StartDate)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(x => x.GroupId)
            .MustAsync((groupId, cancellationToken) =>
            {
                return groupRepository.AnyAsync(x => x.Id == groupId && x.TeacherId == teacherId,
                    cancellationToken);
            })
            .When(x => x.GroupId != null)
            .WithError(Errors.Group.NotFound);

        RuleFor(x => x.StartDate)
            .MustAsync(async (command, startDate, cancellationToken) =>
            {
                return !await holidayRepository.
                    WhereQueryable(x => x.TeacherId == teacherId && x.Id != command.Id )
                    .AnyAsync(
                    x => 
                         x.StartDate <= startDate &&
                         x.EndDate >= command.EndDate ||
                         (x.StartDate >= startDate && x.StartDate <= command.EndDate) ||
                         (x.EndDate >= startDate && x.EndDate <= command.EndDate)
                    , cancellationToken);
            })
            .WithError(Errors.Holiday.ConflictWithOtherHoliday);
    }
}