using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Teachers.Commands.Create;

public class CreateTeacherValidator : AbstractValidator<CreateTeacherCommand>
{
    private readonly ITeacherRepository _teacherRepository;

    public CreateTeacherValidator(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;

        RuleFor(x => x.Name).NotEmpty().Length(Constrains.Teacher.Name);
        RuleFor(x => x.Phone).NotEmpty().Length(Constrains.Phone).Matches("^[0-9]*$");
        RuleFor(x => x.Email).EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .Length(Constrains.Email);
        RuleFor(x => x.SubscriptionPeriodInDays).InclusiveBetween(1, 365);
        RuleFor(x => x.Phone).MustAsync(BeUniquePhone)
            .WithError(Errors.Teacher.PhoneAlreadyExists);
        RuleFor(x => x.Email).MustAsync(BeUniqueEmail)
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithError(Errors.Teacher.EmailAlreadyExists);
    }

    private async Task<bool> BeUniquePhone(string phone, CancellationToken token)
    {
        return !await _teacherRepository.Any(teacher => teacher.Phone == phone, token);
    }

    private async Task<bool> BeUniqueEmail(string? email, CancellationToken token)
    {
        return !await _teacherRepository.Any(teacher => teacher.Email == email, token);
    }
}