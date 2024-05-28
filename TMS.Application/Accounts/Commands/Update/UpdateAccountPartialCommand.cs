using ErrorOr;
using MediatR;
using TMS.Domain.Account;
using TMS.Domain.Groups;
using TMS.Domain.Students;

namespace TMS.Application.Accounts.Commands.Update;

public record UpdateAccountPartialCommand(
    AccountId Id,
    StudentId? StudentId,
    GroupId? GroupId,
    double? BasePrice):
    IRequest<ErrorOr<AccountSummary>>;