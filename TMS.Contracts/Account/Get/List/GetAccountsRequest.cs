using TMS.Application.Common;

namespace TMS.Contracts.Account.Get.List;

public record GetAccountsRequest(string? Search,string? GroupId,bool ?HasWhatsapp,int PageNumber = 1, int PageSize = 10) ;
