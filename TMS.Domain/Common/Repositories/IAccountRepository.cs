using TMS.Domain.Accounts;

namespace TMS.Domain.Common.Repositories;

public interface IAccountRepository: IRepository<Account, AccountId>
{
    Task<Account?> GetIncludeStudentAsync(AccountId accountId, CancellationToken cancellationToken);
}