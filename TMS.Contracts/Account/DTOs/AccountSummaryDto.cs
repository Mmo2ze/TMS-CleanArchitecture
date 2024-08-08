using TMS.Domain.Common.Enums;
using TMS.Domain.Groups;

namespace TMS.Contracts.Account.DTOs;

public record AccountSummaryDto(
    string AccountId,
    string StudentId,
    string? ParentId,
    string? GroupId,
    double BasePrice,
    bool HasCustomPrice,
    bool? HasWhatsapp,
    string StudentName,
    Gender Gender,
    string? GroupName,
    Grade? Grade
);