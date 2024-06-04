using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Sessions;

namespace TMS.Application.Sessions.Commands.Delete;

public class DeleteSessionValidator : AbstractValidator<DeleteSessionCommand>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly ITeacherHelper _teacherHelper;

    public DeleteSessionValidator(ISessionRepository sessionRepository, ITeacherHelper teacherHelper)
    {
        _sessionRepository = sessionRepository;
        _teacherHelper = teacherHelper;

        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Id)
            .MustAsync(BeFound)
            .WithValidationError(ValidationErrors.Session.NotFound);
    }


    private async Task<bool> BeFound(DeleteSessionCommand command, SessionId arg1, CancellationToken arg2)
    {
        return await _sessionRepository.AnyAsync(
            x => x.Id == arg1 &&
                 x.TeacherId == _teacherHelper.GetTeacherId() &&
                 x.GroupId == command.GroupId
            , arg2);
    }
}