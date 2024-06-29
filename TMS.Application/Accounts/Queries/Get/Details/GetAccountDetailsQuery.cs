using ErrorOr;
using MediatR;
using TMS.Domain.Accounts;

namespace TMS.Application.Accounts.Queries.Get.Details;

public record GetAccountDetailsQuery(AccountId Id) : IRequest<ErrorOr<AccountDetailsResult>>;