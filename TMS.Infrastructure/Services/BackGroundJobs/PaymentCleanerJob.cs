using System.Linq.Expressions;
using Coravel.Invocable;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Extensions;
using TMS.Application.Common.Services;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Services.BackGroundJobs;

public class PaymentCleanerJob : IInvocable
{
    private readonly IAccountRepository _accountRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PaymentCleanerJob(IAccountRepository accountRepository, IDateTimeProvider dateTimeProvider)
    {
        _accountRepository = accountRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task Invoke()
    {
        var dateNow = _dateTimeProvider.Now;
        _accountRepository
            .WhereQueryable(x =>
                x.IsPaid == true &&
                x.Payments.Any(p => p.BillDate.Month == dateNow.Month && p.BillDate.Year == dateNow.Year))
            .ExecuteUpdate(setter => setter.SetProperty(x => x.IsPaid, false));
        return Task.CompletedTask;
    }

    private static Expression<Func<Account, bool>> AccountsRequiresModifyQuery(DateTime dateNow)
    {
        return x => x.IsPaid == true && x.Payments.Any(p => p.BillDate.IsSameYearnMonth(dateNow));
    }
}