using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Holidays.Commands.Create;

public class CreateHolidayValidator : AbstractValidator<CreateHolidayCommand>
{
    public CreateHolidayValidator(ITeacherHelper teacherHelper,
        IDateTimeProvider dateTimeProvider, IGroupRepository groupRepository, IHolidayRepository holidayRepository)
    {
        var teacherId = teacherHelper.GetTeacherId();
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
                return !await holidayRepository.AnyAsync(
                    x => x.TeacherId == teacherId &&
                         x.StartDate <= startDate &&
                         x.EndDate >= command.EndDate ||
                         (x.StartDate >= startDate && x.StartDate <= command.EndDate) ||
                         (x.EndDate >= startDate && x.EndDate <= command.EndDate)
                    , cancellationToken);
            })
            .WithError(Errors.Holiday.ConflictWithOtherHoliday);
    }
}