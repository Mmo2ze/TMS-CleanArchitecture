using TMS.Application.Parents.Queries.Get;
using TMS.Application.Students.Queries.GetStudents;
using TMS.Domain.Accounts;
using TMS.Domain.Groups;

namespace TMS.Application.Accounts.Queries.Get.Details;

public record AccountDetailsResult(
    AccountId Id,
    ParentResult? Parent,
    StudentResult Student,
    GroupId? GroupId,
    double BasePrice,
    bool HasCustomPrice,
    bool IsPaid);