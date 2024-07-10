using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Commands.Update;

public class UpdateAccountPartialCommandHandler : IRequestHandler<UpdateAccountPartialCommand, ErrorOr<AccountSummary>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IGroupRepository _groupRepository;

    public UpdateAccountPartialCommandHandler( IAccountRepository accountRepository,
        IGroupRepository groupRepository)
    {
        _accountRepository = accountRepository;
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<AccountSummary>> Handle(UpdateAccountPartialCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetIncludeStudentAsync(request.Id, cancellationToken);

        var groupId = request.GroupId ?? account!.GroupId;
        var group = _groupRepository.GetQueryable()
            .Select(x => new { x.Id, x.BasePrice, x.Grade,x.Name }).FirstOrDefault(g => g.Id == groupId);
        if (group == null)
            return Errors.Group.NotFound;

        account!.Update(request.BasePrice, group.BasePrice, request.GroupId, request.StudentId, request.ParentId,group.Grade);

        return AccountSummary.From(account);
    }
}