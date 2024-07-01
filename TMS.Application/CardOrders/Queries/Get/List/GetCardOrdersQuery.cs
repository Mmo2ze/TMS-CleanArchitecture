using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.Application.CardOrders.Commands.Create;
using TMS.Domain.Common.Models;

namespace TMS.Application.CardOrders.Queries.Get;

public record GetCardOrdersQuery(int Page, int PageSize) : IRequest<ErrorOr<PaginatedList<CardOrderResult>>>;