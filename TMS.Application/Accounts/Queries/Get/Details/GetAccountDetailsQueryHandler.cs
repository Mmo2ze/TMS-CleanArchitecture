using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Application.Parents.Queries.Get;
using TMS.Application.Students.Queries.GetStudents;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Queries.Get.Details;

public class GetAccountDetailsQueryHandler : IRequestHandler<GetAccountDetailsQuery, ErrorOr<AccountDetailsResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;


    public GetAccountDetailsQueryHandler(ITeacherHelper teacherHelper, IAccountRepository accountRepository)
    {
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
    }


    public async Task<ErrorOr<AccountDetailsResult>> Handle(GetAccountDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetQueryable()
            .Include(x => x.Parent)
            .Include(x => x.Student)
            .Select(x => new
            {
                x.Id,
                Parent = x.Parent != null
                    ? new
                    {
                        x.Parent.Id,
                        x.Parent.Name,
                        x.Parent.Email,
                        x.Parent.Phone,
                        x.Parent.Gender
                    }
                    : null,
                Student = new
                {
                    x.Student.Id,
                    x.Student.Name,
                    x.Student.Phone,
                    x.Student.Email,
                    x.Student.Gender
                },
                x.GroupId,
                x.BasePrice,
                x.HasCustomPrice,
                 x.IsPaid
            })
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (account is null)
        {
            return Errors.Account.NotFound;
        }

        return new AccountDetailsResult(
            account.Id,
            account.Parent != null
                ? new ParentResult(
                    account.Parent.Id,
                    account.Parent.Name,
                    account.Parent.Email,
                    account.Parent.Phone,
                    account.Parent.Gender)
                : null,
            new StudentResult(
                account.Student.Id,
                account.Student.Name,
                account.Student.Phone,
                account.Student.Email,
                account.Student.Gender
            ),
            account.GroupId,
            account.BasePrice,
            account.HasCustomPrice,
            account.IsPaid
        );
    }
}