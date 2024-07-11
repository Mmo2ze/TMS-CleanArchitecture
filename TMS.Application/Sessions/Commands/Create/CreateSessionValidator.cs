using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;

namespace TMS.Application.Sessions.Commands.Create;

public class CreateSessionValidator : AbstractValidator<CreateSessionCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly ITeacherHelper _teacherHelper;

    public CreateSessionValidator(IGroupRepository groupRepository, ISessionRepository sessionRepository,
        ITeacherHelper teacherHelper)
    {
        _groupRepository = groupRepository;
        _sessionRepository = sessionRepository;
        _teacherHelper = teacherHelper;


        RuleFor(x => x.GroupId).NotEmpty().WithMessage("GroupId is required.");
        RuleFor(x => x.Day).IsInEnum().WithMessage("invalid Day.");
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("StartTime is required.");
        RuleFor(x => x.EndTime).NotEmpty().WithMessage("EndTime is required.");
        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime).WithMessage("EndTime must be greater than StartTime.");

        RuleFor(x => x.GroupId).MustAsync(BeFoundGroup)
            .WithError(Errors.Group.NotFound);
        RuleFor(x => x.StartTime).MustAsync(BeValidSession)
            .WithError(Errors.Session.SessionIsConflict);
        RuleFor(x => x.Day)
            .MustAsync(HasOneSessionPerDay)
            .WithError(Errors.Session.SessionShouldBeOnePerDay);
    }

    private async Task<bool> HasOneSessionPerDay(CreateSessionCommand command, DayOfWeek arg1, CancellationToken arg2)
    {
        return !await _sessionRepository.AnyAsync(
            x => x.TeacherId == _teacherHelper.GetTeacherId() &&
                 x.GroupId == command.GroupId &&
                 x.Day == command.Day,
            arg2);
    }

    private Task<bool> BeValidSession(CreateSessionCommand command, TimeOnly startTime, CancellationToken token)
    {
        return _sessionRepository.IsValidSessionAsync(command.Day, startTime, command.EndTime, token);
    }

    private async Task<bool> BeFoundGroup(GroupId groupId, CancellationToken token)
    {
        return await _groupRepository.AnyAsync(group => group.Id == groupId, token);
    }
}