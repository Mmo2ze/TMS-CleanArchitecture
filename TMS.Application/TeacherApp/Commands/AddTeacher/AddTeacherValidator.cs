using FluentValidation;
using FluentValidation.Validators;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.TeacherApp.Commands.AddTeacher;

public class AddTeacherValidator : AbstractValidator<AddTeacherCommand>
{
    public AddTeacherValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(6, 26);
        RuleFor(x => x.Phone).NotEmpty().Length(9, 16).Matches("^[0-9]*$");
        RuleFor(x => x.Email).EmailAddress().When(x => string.IsNullOrEmpty(x.Email)).MaximumLength(128);
        RuleFor(x => x.SubscriptionPeriodInDays).InclusiveBetween(1, 365);
    }
}