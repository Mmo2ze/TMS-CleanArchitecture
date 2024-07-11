using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Assistants.Commands.Delete;

public class DeleteAssistantValidator : AbstractValidator<DeleteAssistantCommand>
{
    private readonly IAssistantRepository _assistantRepository;
    private readonly ITeacherHelper _teacherHelper;

    public DeleteAssistantValidator(IAssistantRepository assistantRepository, ITeacherHelper teacherHelper)
    {
        _assistantRepository = assistantRepository;
        _teacherHelper = teacherHelper;

        RuleFor(x => x.Id)
            .MustAsync(BeFoundAssistant)
            .WithError(Errors.Assistant.NotFound);
    }

    private Task<bool> BeFoundAssistant(AssistantId arg1, CancellationToken arg2)
    {
        return _assistantRepository
            .AnyAsync(x => x.Id == arg1 && x.TeacherId == _teacherHelper.GetTeacherId(), arg2);
    }
}