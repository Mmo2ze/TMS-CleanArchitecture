using System.Runtime.CompilerServices;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Queries.Get;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery,ErrorOr< PaginatedList<AccountSummary>>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IGroupRepository _groupRepository;
    private readonly IAccountRepository _accountRepository;

    public GetAccountsQueryHandler(ITeacherHelper teacherHelper, IAccountRepository accountRepository, IGroupRepository groupRepository)
    {
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
        _groupRepository = groupRepository;
    }

    public async Task<ErrorOr<PaginatedList<AccountSummary>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var accounts = _accountRepository.GetQueryable()
            .Include(a => a.Student)
            .Include(x => x.Group)
            .Where(a => a.TeacherId == _teacherHelper.GetTeacherId() &&
                        (string.IsNullOrEmpty(request.Search) ||
                         EF.Functions.Like(a.Student.Name, $"%{request.Search}%") ||
                         EF.Functions.Like(a.Student.Phone, $"%{request.Search}%")) &&
                        (request.GroupId == null || a.GroupId == request.GroupId))
            .OrderBy(x => x.Student.Name);
        var response = accounts.Select(x=> AccountSummary.From(x));
        var result = await response.PaginatedListAsync(request.PageNumber, request.PageSize);
        return result;
    }
}