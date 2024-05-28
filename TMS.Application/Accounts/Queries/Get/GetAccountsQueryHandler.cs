using System.Runtime.CompilerServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Domain.Account;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Queries.Get;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, PaginatedList<AccountSummary>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;

    public GetAccountsQueryHandler(ITeacherHelper teacherHelper, IAccountRepository accountRepository)
    {
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
    }

    public async Task<PaginatedList<AccountSummary>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = _accountRepository.GetQueryable()
            .Include(a => a.Student)
            .Where(a => a.TeacherId == _teacherHelper.GetTeacherId() &&
                        (string.IsNullOrEmpty(request.Search) ||
                         EF.Functions.Like(a.Student.Name, $"%{request.Search}%") ||
                         EF.Functions.Like(a.Student.Phone, $"%{request.Search}%")) &&
                        (request.GroupId == null || a.GroupId == request.GroupId))
            .OrderBy(x => x.BasePrice)
            .Select(account => new AccountSummary(
                account.Id,
                account.StudentId,
                account.GroupId,
                account.BasePrice,
                account.HasCustomPrice,
                account.Student.Name,
                account.Student.Gender
            ));

        var result = await PaginatedList<AccountSummary>.CreateAsync(accounts, request.PageNumber, request.PageSize);
        return result;
    }
}