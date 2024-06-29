using ErrorOr;
using MediatR;
using TMS.Application.Payments.Commands.Create;
using TMS.Domain.Accounts;
using TMS.Domain.Common.Models;

namespace TMS.Application.Payments.Queries.Get;

public record GetAccountPaymentsQuery(
    int Page,
    int PageSize,
    AccountId Id
    ) : IRequest<ErrorOr<PaginatedList<PaymentResult>>>;