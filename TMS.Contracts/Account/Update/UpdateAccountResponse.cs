using TMS.Contracts.Account.DTOs;
using TMS.Domain.Account;

namespace TMS.Contracts.Account.Update;

public record UpdateAccountResponse(AccountSummaryDto AccountSummary);