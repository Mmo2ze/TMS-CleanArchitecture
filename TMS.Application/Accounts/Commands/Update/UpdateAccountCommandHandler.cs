using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Commands.Update;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, ErrorOr<AccountSummary>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IGroupRepository _groupRepository;

    public UpdateAccountCommandHandler(IAccountRepository accountRepository,
        IGroupRepository groupRepository)
    {
        _accountRepository = accountRepository;
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<AccountSummary>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetIncludeStudentAsync(request.Id, cancellationToken);

        var group = _groupRepository.GetQueryable()
            .Select(x => new { x.Id, x.BasePrice, x.Grade }).FirstOrDefault(g => g.Id == request.GroupId);
        if (group == null)
            return Errors.Group.NotFound;

        account!.Update(request.BasePrice, group.BasePrice, request.GroupId, request.StudentId, request.ParentId,group.Grade);

        return AccountSummary.From(account);
    }
}