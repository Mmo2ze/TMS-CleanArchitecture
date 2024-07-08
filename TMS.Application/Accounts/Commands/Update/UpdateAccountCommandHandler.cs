using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Accounts.Queries.Get.Details;
using TMS.Application.Parents.Queries.Get;
using TMS.Application.Students.Queries.GetStudents;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Commands.Update;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, ErrorOr<AccountDetailsResult>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IUnitOfWork _unitOfWork;


    public UpdateAccountCommandHandler(IAccountRepository accountRepository,
        IGroupRepository groupRepository, IUnitOfWork unitOfWork)
    {
        _accountRepository = accountRepository;
        _groupRepository = groupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<AccountDetailsResult>> Handle(UpdateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetQueryable()
            .Include(x => x.Student)
            .Include(x => x.Parent)
            .FirstAsync(x => x.Id == request.Id, cancellationToken);

        var group = _groupRepository.GetQueryable()
            .Select(x => new { x.Id, x.BasePrice, x.Grade }).FirstOrDefault(g => g.Id == request.GroupId);
        if (group == null)
            return Errors.Group.NotFound;

        account.Update(request.BasePrice, group.BasePrice, request.GroupId, request.StudentId, request.ParentId,
            group.Grade);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        account = await _accountRepository.GetQueryable()
            .Include(x => x.Student)
            .Include(x => x.Parent)
            .FirstAsync(x => x.Id == request.Id, cancellationToken);
        return new AccountDetailsResult(
            account.Id,
            account.Parent != null
                ? new ParentResult(
                    account.Parent.Id,
                    account.Parent.Name,
                    account.Parent.Email,
                    account.Parent.Phone,
                    account.Parent.Gender,
                    account.Parent.HasWhatsapp)
                : null,
            new StudentResult(
                account.Student.Id,
                account.Student.Name,
                account.Student.Phone,
                account.Student.Email,
                account.Student.Gender,
                account.Student.HasWhatsapp
            ),
            account.GroupId,
            account.BasePrice,
            account.HasCustomPrice,
            account.IsPaid
        );
    }
}