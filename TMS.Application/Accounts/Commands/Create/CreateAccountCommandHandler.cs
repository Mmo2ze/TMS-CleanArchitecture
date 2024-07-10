using ErrorOr;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Accounts.Queries.Get.Details;
using TMS.Application.Common.Services;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Application.Accounts.Commands.Create;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ErrorOr<AccountDetailsResult>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IGroupRepository groupRepository, ITeacherHelper teacherHelper,
        IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<AccountDetailsResult>> Handle(CreateAccountCommand request,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetQueryable()
                .Include(x => x.Group)
                .Include(x => x.Student)
                .Include(x => x.Parent)
                .FirstOrDefaultAsync(x => x.StudentId == request.StudentId &&
                                          x.TeacherId == _teacherHelper.GetTeacherId(),
                    cancellationToken);
        var group = await _groupRepository.FirstAsync(g => g.Id == request.GroupId, cancellationToken);
        if (account == null)
        {
            var newAccount = Account.Create(request.StudentId, group.BasePrice, group.Id, group.TeacherId,
                group.Grade, request.ParentId);
            group.AddStudent(newAccount);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
             newAccount = await _accountRepository.GetIncludeStudentAsync(newAccount.Id, cancellationToken);
             if(newAccount is null)
                 return Error.Failure("something went wrong");
            return AccountDetailsResult.From(newAccount);
        }

        group.AddStudent(account);
        return AccountDetailsResult.From(account,group.Id);
    }
}