using TMS.Application.Common;
using TMS.Domain.Groups;

namespace TMS.Contracts.Account.Get.List;

public record GetAccountsRequest(string? Search,string? GroupId) : GetPaginatedList;
