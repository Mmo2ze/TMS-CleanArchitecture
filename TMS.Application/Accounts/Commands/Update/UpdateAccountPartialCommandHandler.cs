using ErrorOr;
using MediatR;
using TMS.Domain.Account;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Commands.Update;

public class UpdateAccountPartialCommandHandler : IRequestHandler<UpdateAccountPartialCommand, ErrorOr<AccountSummary>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAccountPartialCommandHandler(IUnitOfWork unitOfWork, IAccountRepository accountRepository,
        IGroupRepository groupRepository)
    {
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<AccountSummary>> Handle(UpdateAccountPartialCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetIncludeStudentAsync(request.Id, cancellationToken);

        var groupId = request.GroupId ?? account!.GroupId;
        var groupPrice = _groupRepository.GetQueryable().FirstOrDefault(g => g.Id == groupId)?.BasePrice;
        if (groupPrice == null)
            return Errors.Group.NotFound;
        
        var accountPrice  = request.BasePrice ?? groupPrice.Value;
        account!.Update(accountPrice, groupPrice.Value, request.GroupId, request.StudentId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AccountSummary.From(account);
    }
}