using TMS.Domain.Account;
using TMS.Domain.Students;

namespace TMS.Domain.Common.Repositories;

public interface IAccountRepository: IRepository<Account.Account, AccountId>
{
    Task<Account.Account?> GetIncludeStudentAsync(AccountId accountId, CancellationToken cancellationToken);
}