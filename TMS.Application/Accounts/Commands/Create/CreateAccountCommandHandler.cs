using ErrorOr;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Application.Accounts.Commands.Create;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, ErrorOr<AccountSummary>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandler(IGroupRepository groupRepository, ITeacherHelper teacherHelper,
        IUnitOfWork unitOfWork, IAccountRepository accountRepository, IStudentRepository studentRepository)
    {
        _groupRepository = groupRepository;
        _teacherHelper = teacherHelper;
        _unitOfWork = unitOfWork;
        _accountRepository = accountRepository;
        _studentRepository = studentRepository;
    }

    public async Task<ErrorOr<AccountSummary>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetQueryable()
                .Include(x => x.Student)
                .FirstOrDefaultAsync(x => x.StudentId == request.StudentId &&
                                          x.TeacherId == _teacherHelper.GetTeacherId(),
                    cancellationToken)
                .Select(x => new { x.Student.Name, x.Student.Gender, Value = x })
            ;
        var group = await _groupRepository.FirstAsync(g => g.Id == request.GroupId, cancellationToken);
        if (account?.Value == null)
        {
            var newAccount = Account.Create(request.StudentId, group.BasePrice, group.Id, group.TeacherId,
                request.ParentId);
            group.AddStudent(newAccount);
            var student = await _studentRepository.FirstAsync(x => x.Id == request.StudentId, cancellationToken)
                .Select(x => new { x.Name, x.Gender });
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new AccountSummary(newAccount.Id, newAccount.StudentId,newAccount.ParentId, group.Id, newAccount.BasePrice,
                newAccount.HasCustomPrice, student.Name,
                student.Gender);
        }

        group.AddStudent(account.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new AccountSummary(account.Value.Id, account.Value.StudentId,account.Value.ParentId, group.Id, account.Value.BasePrice,
            account.Value.HasCustomPrice, account.Name, account.Gender);
    }
}