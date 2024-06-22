using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Domain.Common.Constrains;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Parents.Commands.Create;

public class CreateParentValidator : AbstractValidator<CreateParentComamnd>
{
    private readonly IParentRepository _parentRepository;
    public CreateParentValidator(IParentRepository parentRepository)
    {
        _parentRepository = parentRepository;
        RuleFor(x => x.Name).NotEmpty().Length(Constrains.Student.Name);


        RuleFor(x => x.Phone)!.Length(Constrains.Phone).Matches("^[0-9]*$")
            .When(x => x.Phone != null);
        RuleFor(x => x.Phone).MustAsync(PhoneNotUsed!)
            .WithValidationError(Errors.Parnet.PhoneAlreadyExists);
        RuleFor(x => x.Email).EmailAddress()
            .When(x => x.Email != null)
            .Length(Constrains.Email);
    }

    private async Task<bool> PhoneNotUsed(string arg1, CancellationToken arg2)
    {
        return !await _parentRepository.AnyAsync(x=>x.Phone == arg1, arg2);
    }
}