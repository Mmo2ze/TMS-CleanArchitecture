using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.ValidationErrors;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Parents;

namespace TMS.Application.Students.Commands.Create;

public class CreateStudentValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(Constrains.Student.Name);

       

        RuleFor(x => x.Phone)!.Length(Constrains.Phone).Matches("^[0-9]*$")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Email).EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .Length(Constrains.Email);
    }

    
}