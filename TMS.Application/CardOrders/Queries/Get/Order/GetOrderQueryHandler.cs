using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.CardOrders.Commands.Create;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Models;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.CardOrders.Queries.Get.Order;

public class GetOrderQueryHandler : IRequestHandler<GetCardOrderQuery, ErrorOr<CardOrderDetailsResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly IClaimsReader _claimsReader;
    private readonly ICardOrderRepository _cardOrderRepository;
    private readonly IAccountRepository _accountRepository;

    public GetOrderQueryHandler(ITeacherHelper teacherHelper, ICardOrderRepository cardOrderRepository,
        IAccountRepository accountRepository, IClaimsReader claimsReader)
    {
        _teacherHelper = teacherHelper;
        _cardOrderRepository = cardOrderRepository;
        _accountRepository = accountRepository;
        _claimsReader = claimsReader;
    }

    public async Task<ErrorOr<CardOrderDetailsResult>> Handle(GetCardOrderQuery request,
        CancellationToken cancellationToken)
    {
        var role = _claimsReader.GetRoles();
        var order = await _cardOrderRepository.WhereQueryable(x =>
                x.Id == request.Id && (role.Contains(Roles.Admin.Role) || x.TeacherId == _teacherHelper.GetTeacherId()))
            .Include(x => x.AccountIds)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (order is null)
            return Errors.CardOrder.NotFound;
        var shortAccountIds = _accountRepository.WhereQueryable(x => order.AccountIds.Contains(x.Id))
            .Include(x => x.Student)
            .Select(x => new ShortAccount(x.Id, x.Student.Name)).ToList();
        return new CardOrderDetailsResult(
            order.Id,
            order.TeacherId,
            order.TeacherName,
            order.CreatedAt,
            order.CompletedAt,
            order.CancelledAt,
            order.AcceptedBy,
            order.CancelledBy,
            order.AccountIds.Count,
            order.Status,
            shortAccountIds);
    }
}