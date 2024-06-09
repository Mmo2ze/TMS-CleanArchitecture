using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Domain.Common.Constrains;

namespace TMS.Application.Parents.Commands.Create;

public class CreateParentValidator: AbstractValidator<CreateParentComamnd>
{
    public CreateParentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(Constrains.Student.Name);


        RuleFor(x => x.Phone)!.Length(Constrains.Phone).Matches("^[0-9]*$")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.Email).EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .Length(Constrains.Email);
    }
}