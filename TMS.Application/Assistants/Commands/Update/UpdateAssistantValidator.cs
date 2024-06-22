using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Assistants;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Assistants.Commands.Update;

public class UpdateAssistantValidator : AbstractValidator<UpdateAssistantCommand>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAssistantRepository _assistantRepository;

    public UpdateAssistantValidator(IAssistantRepository assistantRepository, ITeacherHelper teacherHelper)
    {
        _assistantRepository = assistantRepository;
        _teacherHelper = teacherHelper;

        RuleFor(x => x.Email).EmailAddress().When(x => x.Email != null);
        RuleFor(x => x.Phone).NotEmpty().Length(Constrains.Phone).Matches("^[0-9]*$");
        RuleFor(x => x.Name).NotEmpty().Length(Constrains.Assistant.Name);
        
        
        RuleFor(x => x.Id)
            .MustAsync(BeFoundAssistant)
            .WithValidationError(Errors.Assistant.NotFound);
        RuleFor(x => x.Phone)
            .MustAsync(NotBeUsedPhone)
            .WithValidationError(Errors.Assistant.PhoneAlreadyExists);
        
        
        RuleFor(x => x.Email)
            .MustAsync(NotBeUsedEmail!)
            .When(x => x.Email != null)
            .WithValidationError(Errors.Assistant.EmailAlreadyExists);
    }

    private async Task<bool> NotBeUsedEmail(UpdateAssistantCommand command,string arg1, CancellationToken arg2)
    {
        return !await _assistantRepository.AnyAsync(assistant => assistant.Email == arg1 && assistant.Id != command.Id, arg2);
    }

    private Task<bool> BeFoundAssistant(AssistantId arg1, CancellationToken arg2)
    {
        return _assistantRepository.AnyAsync(
            assistant => assistant.Id == arg1 && assistant.TeacherId == _teacherHelper.GetTeacherId(), arg2);
    }

    private async Task<bool> NotBeUsedPhone(UpdateAssistantCommand command,string arg1, CancellationToken arg2)
    {
       return !await _assistantRepository.AnyAsync(assistant => assistant.Phone == arg1 && assistant.Id != command.Id, arg2);
    }
    
    
    
}