using TMS.Domain.Common.Enums;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Students;

namespace TMS.Domain.Accounts;

public record AccountSummary(
    AccountId AccountId,
    StudentId StudentId,
    ParentId? ParentId,
    GroupId GroupId,
    double BasePrice,
    bool HasCustomPrice,
    string StudentName,
    Gender Gender)
{
    public static AccountSummary From(Account account) =>
        new(
            account.Id,
            account.StudentId,
            account.ParentId,
            account.GroupId,
            account.BasePrice,
            account.HasCustomPrice,
            account.Student.Name,
            account.Student.Gender
        );
};