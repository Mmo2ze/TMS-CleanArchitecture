using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Parents;

namespace TMS.Application.Students.Commands.Create;

public class CreateStudentValidator : AbstractValidator<CreateStudentCommand>
{
    private readonly IStudentRepository _studentRepository;
    public CreateStudentValidator(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
        RuleFor(x => x.Name).NotEmpty().Length(Constrains.Student.Name);

        
        RuleFor(x => x.Phone)!.Length(Constrains.Phone).Matches("^[0-9]*$")
            .When(x => x.Phone != null);
        RuleFor(x => x.Phone).MustAsync(PhoneNotUsed!)
            .WithError(Errors.Student.PhoneAlreadyExists)
            .When(x => x.Phone != null);
        RuleFor(x => x.Email).EmailAddress()
            .When(x => x.Email != null)
            .Length(Constrains.Email);
    }

    private async Task<bool> PhoneNotUsed(string arg1, CancellationToken arg2)
    {
        return ! await _studentRepository.AnyAsync(x => x.Phone == arg1, arg2);
    }
}