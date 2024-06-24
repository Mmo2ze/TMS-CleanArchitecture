using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;
using TMS.Domain.Groups;

namespace TMS.Application.Accounts.Commands.Delete;

public record DeleteAccountCommand(AccountId Id,GroupId GroupId) : IRequest<ErrorOr<string>>;