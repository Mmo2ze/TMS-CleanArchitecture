using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.Accounts.Queries.Get;

public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, ErrorOr<PaginatedList<AccountSummary>>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IAccountRepository _accountRepository;

    public GetAccountsQueryHandler(ITeacherHelper teacherHelper, IAccountRepository accountRepository)
    {
        _teacherHelper = teacherHelper;
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<PaginatedList<AccountSummary>>> Handle(GetAccountsQuery request,
        CancellationToken cancellationToken)
    {
        var teacherId = _teacherHelper.GetTeacherId();
        var accounts = _accountRepository.GetQueryable()
            .Include(a => a.Student)
            .Include(a => a.Group)
            .Where(a =>
                //filtering by teacherId
                a.TeacherId == teacherId &&

                // get accounts that in group
                a.GroupId != null &&

                //filtering by the presence of whatsapp
                (!request.HasWhatsapp.HasValue || a.Student.HasWhatsapp == request.HasWhatsapp) &&

                //filtering by payment status
                (!request.IsPaid.HasValue || a.IsPaid == request.IsPaid) &&

                // filtering by attendance status
                (request.AttendanceStatus == null ||
                 (a.AttendanceStatus.HasValue && request.AttendanceStatus == a.AttendanceStatus.Value)) &&

                // filtering by search term 
                (string.IsNullOrEmpty(request.Search) ||
                 EF.Functions.Like(a.Student.Name, $"%{request.Search}%") ||
                 EF.Functions.Like(a.Student.Phone, $"%{request.Search}%")) &&

                // filtering by group id
                (request.GroupId == null || a.GroupId == request.GroupId))
            .OrderBy(a => a.Student.Name);
        var response = accounts.Select(x => AccountSummary.From(x));
        var result = await response.PaginatedListAsync(request.PageNumber, request.PageSize);
        return result;
    }
}