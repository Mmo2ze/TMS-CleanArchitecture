using Microsoft.EntityFrameworkCore;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Students;

namespace TMS.Infrastructure.Persistence.Repositories;

public class AccountRepository : Repository<Account, AccountId>, IAccountRepository
{
    public AccountRepository(MainContext dbContext) : base(dbContext)
    {
    }

    public Task<Account?> GetIncludeStudentAsync(AccountId accountId, CancellationToken cancellationToken)
    {
        return DbContext.Accounts
            .Include(a => a.Student)
            .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);
    }
}