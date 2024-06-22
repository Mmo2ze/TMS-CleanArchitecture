using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;

namespace TMS.Application.Sessions.Commands.Create;

public class CreateSessionValidator : AbstractValidator<CreateSessionCommand>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ISessionRepository _sessionRepository;

    public CreateSessionValidator(IGroupRepository groupRepository, ISessionRepository sessionRepository)
    {
        _groupRepository = groupRepository;
        _sessionRepository = sessionRepository;

        RuleFor(x => x.GroupId).NotEmpty().WithMessage("GroupId is required.");
        RuleFor(x => x.Day).IsInEnum().WithMessage("invalid Day.");
        RuleFor(x => x.StartTime).NotEmpty().WithMessage("StartTime is required.");
        RuleFor(x => x.EndTime).NotEmpty().WithMessage("EndTime is required.");
        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime).WithMessage("EndTime must be greater than StartTime.");

        RuleFor(x => x.GroupId).MustAsync(BeFoundGroup)
            .WithError(Errors.Group.NotFound);
        RuleFor(x => x.Day).MustAsync(BeValidSession)
            .WithError(Errors.Session.SessionIsConflict);
    }

    private Task<bool> BeValidSession(CreateSessionCommand command, DayOfWeek day, CancellationToken token)
    {
        return _sessionRepository.IsValidSessionAsync(day, command.StartTime, command.EndTime, token);
    }

    private async Task<bool> BeFoundGroup(GroupId groupId, CancellationToken token)
    {
        return await _groupRepository.AnyAsync(group => group.Id == groupId, token);
    }
}