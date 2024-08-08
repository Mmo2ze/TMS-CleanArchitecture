using TMS.Domain.Common.Enums;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Students;

namespace TMS.Domain.Accounts;

public record AccountSummary(
    AccountId AccountId,
    StudentId StudentId,
    ParentId? ParentId,
    GroupId? GroupId,
    double BasePrice,
    bool HasCustomPrice,
    bool? HasWhatsapp,
    string StudentName,
    Gender Gender,
    bool IsPaid,
    string? GroupName,
    Grade ?Grade)
{
    public static AccountSummary From(Account account) =>
        new(
            account.Id,
            account.StudentId,
            account.ParentId,
            account.GroupId,
            account.BasePrice,
            account.HasCustomPrice,
            account.Student.HasWhatsapp,
            account.Student.Name,
            account.Student.Gender,
            account.IsPaid,
            account.Group?.Name,
            account.Group?.Grade
        );
}

public record GroupSummary(GroupId GroupId, string Name, Grade Grade)
{
    public static GroupSummary From(Group group) =>
        new(group.Id, group.Name, group.Grade);
};