using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Students;

namespace TMS.Application.Accounts.Commands.Update;

public record UpdateAccountPartialCommand(
    AccountId Id,
    StudentId? StudentId,
    GroupId? GroupId,
    double? BasePrice,
    ParentId? ParentId) :
    IRequest<ErrorOr<AccountSummary>>;