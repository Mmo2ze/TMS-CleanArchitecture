using TMS.Domain.Accounts;
using TMS.Domain.Students;

namespace TMS.Domain.Common.Repositories;

public interface IAccountRepository: IRepository<Account, AccountId>
{
    Task<Account?> GetIncludeStudentAsync(AccountId accountId, CancellationToken cancellationToken);
}