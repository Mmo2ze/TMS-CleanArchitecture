using TMS.Contracts.Account.DTOs;
using TMS.Domain.Account;
using TMS.Domain.Common.Models;

namespace TMS.Contracts.Account.Get.List;

public record GetAccountsResponse(PaginatedList<AccountSummaryDto> Accounts);