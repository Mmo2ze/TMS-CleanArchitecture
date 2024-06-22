using TMS.Domain.Common.Enums;

namespace TMS.Contracts.Account.DTOs;

public record AccountSummaryDto(
    string AccountId,
    string StudentId,
    string ParentId,
    string GroupId,
    double BasePrice,
    bool HasCustomPrice,
    string StudentName,
    Gender Gender);