using ErrorOr;
using MediatR;
using TMS.Domain.Account;
using TMS.Domain.Groups;
using TMS.Domain.Students;

namespace TMS.Application.Accounts.Commands.Create;

public record CreateAccountCommand(GroupId GroupId, StudentId StudentId) : IRequest<ErrorOr<AccountId>>;