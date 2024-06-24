using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Groups;
using TMS.Domain.Parents;
using TMS.Domain.Students;

namespace TMS.Application.Accounts.Commands.Create;

public record CreateAccountCommand(GroupId GroupId, StudentId StudentId,ParentId? ParentId) : IRequest<ErrorOr<AccountSummary>>;