using System.Security.Cryptography;
using TMS.Domain.Common.Enums;
using TMS.Domain.Groups;
using TMS.Domain.Students;

namespace TMS.Domain.Account;

public record AccountSummary(
    AccountId AccountId,
    StudentId StudentId,
    GroupId GroupId,
    double BasePrice,
    bool HasCustomPrice,
    string StudentName,
    Gender Gender)
{
    public static AccountSummary From(Account account) =>
        new AccountSummary(
            account.Id,
            account.StudentId,
            account.GroupId,
            account.BasePrice,
            account.HasCustomPrice,
            account.Student.Name,
            account.Student.Gender
        );
};