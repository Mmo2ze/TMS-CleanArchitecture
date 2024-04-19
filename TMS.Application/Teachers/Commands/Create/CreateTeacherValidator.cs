﻿using FluentValidation;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Teachers.Commands.Create;

public class CreateTeacherValidator : AbstractValidator<CreateTeacherCommand>
{
    private readonly ITeacherRepository _teacherRepository;

    public CreateTeacherValidator(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;

        RuleFor(x => x.Name).NotEmpty().Length(6, 26);
        RuleFor(x => x.Phone).NotEmpty().Length(9, 16).Matches("^[0-9]*$");
        RuleFor(x => x.Email).EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .MaximumLength(128);
        RuleFor(x => x.SubscriptionPeriodInDays).InclusiveBetween(1, 365);
        RuleFor(x => x.Phone).MustAsync(BeUniquePhone)
            .WithValidationError(ValidationErrors.Teacher.PhoneAlreadyExists);
        RuleFor(x => x.Email).MustAsync(BeUniqueEmail)
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithValidationError(ValidationErrors.Teacher.EmailAlreadyExists);
    }

    private async Task<bool> BeUniquePhone(string phone, CancellationToken token)
    {
        return !await _teacherRepository.Any(teacher => teacher.Phone == phone, token);
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken token)
    {
        return !await _teacherRepository.Any(teacher => teacher.Email == email, token);
    }
}