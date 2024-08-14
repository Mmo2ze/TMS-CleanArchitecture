using TMS.Application.Common;
using TMS.Domain.Attendances;

namespace TMS.Contracts.Account.Get.List;

public record GetAccountsRequest(
    string? Search,
    string? GroupId,
    bool? HasWhatsapp,
    bool? IsPaid,
    AttendanceStatus? AttendanceStatus,
    int PageNumber = 1,
    int PageSize = 10);