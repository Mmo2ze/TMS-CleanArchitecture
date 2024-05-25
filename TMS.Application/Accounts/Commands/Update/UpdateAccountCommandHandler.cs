using ErrorOr;
using MediatR;
using TMS.Domain.Account;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Commands.Update;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, ErrorOr<AccountSummary>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork,
        IGroupRepository groupRepository)
    {
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<AccountSummary>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetIncludeStudentAsync(request.Id, cancellationToken);
        
        var groupPrice = _groupRepository.GetQueryable().FirstOrDefault(g => g.Id == request.GroupId)?.BasePrice;
        if (groupPrice == null)
            return Errors.Group.NotFound;

        account!.Update(request.BasePrice, groupPrice.Value, request.GroupId, request.StudentId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return AccountSummary.From(account);
    }
}