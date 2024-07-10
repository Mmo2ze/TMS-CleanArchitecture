using TMS.Application.Parents.Queries.Get;
using TMS.Application.Students.Queries.GetStudents;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Errors;
using TMS.Domain.Groups;

namespace TMS.Application.Accounts.Queries.Get.Details;

public record AccountDetailsResult(
    AccountId Id,
    ParentResult? Parent,
    StudentResult Student,
    GroupId? GroupId,
    double BasePrice,
    bool HasCustomPrice,
    bool IsPaid)
{
    public static AccountDetailsResult From(Account account,GroupId? groupId = null)
    {
        return new AccountDetailsResult(
            account.Id,
            account.Parent != null
                ? new ParentResult(
                    account.Parent.Id,
                    account.Parent.Name,
                    account.Parent.Email,
                    account.Parent.Phone,
                    account.Parent.Gender,
                    account.Parent.HasWhatsapp)
                : null,
            new StudentResult(
                account.Student.Id,
                account.Student.Name,
                account.Student.Phone,
                account.Student.Email,
                account.Student.Gender,
                account.Student.HasWhatsapp
            ),
            account.GroupId??groupId,
            account.BasePrice,
            account.HasCustomPrice,
            account.IsPaid
        );
    }
};