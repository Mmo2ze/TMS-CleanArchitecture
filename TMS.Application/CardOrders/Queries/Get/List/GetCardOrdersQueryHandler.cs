using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.CardOrders.Commands.Create;
using TMS.Application.Common.Mapping;
using TMS.Application.Common.Services;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.CardOrders.Queries.Get;

public class GetCardOrdersQueryHandler : IRequestHandler<GetCardOrdersQuery, ErrorOr<PaginatedList<CardOrderResult>>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IClaimsReader _claimsReader;
    private readonly ICardOrderRepository _cardOrderRepository;

    public GetCardOrdersQueryHandler(ICardOrderRepository cardOrderRepository, IClaimsReader claimsReader,
        ITeacherHelper teacherHelper)
    {
        _cardOrderRepository = cardOrderRepository;
        _claimsReader = claimsReader;
        _teacherHelper = teacherHelper;
    }

    public async Task<ErrorOr<PaginatedList<CardOrderResult>>> Handle(GetCardOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var role = _claimsReader.GetRoles();
        IQueryable<CardOrderResult>? orders = null;
        if (role.Contains("Admin"))
        {
            orders = _cardOrderRepository.GetQueryable()
                .Include(x => x.AccountIds)
                .Select(x => new CardOrderResult(
                    x.Id,
                    x.AccountIds,
                    x.TeacherId,
                    x.TeacherName,
                    x.CreatedAt,
                    x.AcceptedAt,
                    x.CancelledAt,
                    x.AcceptedBy,
                    x.CancelledBy,
                    x.AccountIds.Count,
                    x.Status));
        }
        else
        {
            var teacherId = _teacherHelper.GetTeacherId();
            orders = _cardOrderRepository.WhereQueryable(x => x.TeacherId == teacherId)
                .Include(x => x.AccountIds)
                .Select(x => new CardOrderResult(
                    x.Id,
                    x.AccountIds,
                    x.TeacherId,
                    x.TeacherName,
                    x.CreatedAt,
                    x.AcceptedAt,
                    x.CancelledAt,
                    x.AcceptedBy,
                    x.CancelledBy,
                    x.AccountIds.Count,
                    x.Status));
        }

        return await orders.PaginatedListAsync(request.Page, request.PageSize);
    }
}