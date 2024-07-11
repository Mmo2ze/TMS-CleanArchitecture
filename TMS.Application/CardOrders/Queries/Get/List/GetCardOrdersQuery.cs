using ErrorOr;
using MediatR;
using TMS.Application.CardOrders.Commands.Create;
using TMS.Domain.Common.Models;

namespace TMS.Application.CardOrders.Queries.Get.List;

public record GetCardOrdersQuery(int Page, int PageSize) : IRequest<ErrorOr<PaginatedList<CardOrderResult>>>;