using ErrorOr;
using MediatR;
using TMS.Domain.Account;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Students;

namespace TMS.Application.Accounts.Commands.Update;

public record UpdateAccountCommand(
    AccountId Id,
    StudentId StudentId,
    GroupId GroupId,
    double BasePrice,
    ParentId? ParentId)
    : IRequest<ErrorOr<AccountSummary>>;