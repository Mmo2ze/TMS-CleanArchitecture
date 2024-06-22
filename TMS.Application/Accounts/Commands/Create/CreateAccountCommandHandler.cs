using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Account;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Application.Accounts.Commands.Create;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ErrorOr<AccountSummary>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IGroupRepository groupRepository, ITeacherHelper teacherHelper,
        IUnitOfWork unitOfWork, IAccountRepository accountRepository)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<AccountSummary>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetQueryable()
            .Include(x => x.Student)
            .FirstOrDefaultAsync(x => x.StudentId == request.StudentId &&
                                      x.TeacherId == _teacherHelper.GetTeacherId(),
                cancellationToken);
        var group = await _groupRepository.FirstAsync(g => g.Id == request.GroupId, cancellationToken);
        account ??= Account.Create(request.StudentId, group.BasePrice, group.Id, group.TeacherId, request.ParentId);
        group.AddStudent(account);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return AccountSummary.From(account);
    }
}