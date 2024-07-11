using FluentValidation;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;

namespace TMS.Application.Accounts.Queries.Get;

public class GetAccountsValidator: AbstractValidator<GetAccountsQuery>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ITeacherHelper _teacherHelper;
    
    public GetAccountsValidator(IGroupRepository groupRepository, ITeacherHelper teacherHelper)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be greater than or equal to 1");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be greater than or equal to 1");
        
        RuleFor(x => x.GroupId).MustAsync(BeFound)
            .WithError(Errors.Group.NotFound).When(x => x.GroupId is not null);
    }

    private async Task<bool> BeFound(GroupId arg1, CancellationToken arg2)
    {
        return await _groupRepository.AnyAsync(
            x => x.Id == arg1 &&
                 x.TeacherId == _teacherHelper.GetTeacherId()
            , arg2);
    }
}