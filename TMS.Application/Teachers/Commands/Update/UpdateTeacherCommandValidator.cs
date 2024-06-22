using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Teachers.Commands.Update;

public class UpdateTeacherCommandValidator : AbstractValidator<UpdateTeacherCommand>
{
    private readonly ITeacherRepository _teacherRepository;

    public UpdateTeacherCommandValidator(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
        RuleFor(x => x.Name).NotEmpty().Length(6, 26);
        RuleFor(x => x.Phone).NotEmpty().Length(9, 16).Matches("^[0-9]*$");
        RuleFor(x => x.Email).EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .MaximumLength(128);
        RuleFor(x => x.Phone).MustAsync(BeUniquePhone)
            .WithError(Errors.Teacher.PhoneAlreadyExists);
        RuleFor(x => x.Email).MustAsync(BeUniqueEmail)
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithError(Errors.Teacher.EmailAlreadyExists);
    }

    private async Task<bool> BeUniquePhone(UpdateTeacherCommand command, string phone,
        CancellationToken cancellationToken)
    {
        return !await _teacherRepository.Any(teacher => teacher.Phone == phone && teacher.Id != command.TeacherId,
            cancellationToken);
    }


    private async Task<bool> BeUniqueEmail(UpdateTeacherCommand command, string? email, CancellationToken token)
    {
            return !await _teacherRepository.Any(teacher => teacher.Email == email && teacher.Id != command.TeacherId,
                token);
    }
}