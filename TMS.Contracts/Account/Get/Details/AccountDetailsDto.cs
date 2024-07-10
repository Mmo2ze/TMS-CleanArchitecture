using TMS.Contracts.Parent.Get;
using TMS.Contracts.Student.Get;
using TMS.Domain.Common.Enums;

namespace TMS.Contracts.Account.Get.Details;

public record AccountDetailsDto(
    string Id,
    ParentDto Parent,
    StudentDto Student,
    string? GroupId,
    double BasePrice,
    bool HasCustomPrice,
    bool IsPaid);