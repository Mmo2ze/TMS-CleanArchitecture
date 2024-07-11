using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Common.Errors;
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
            .Include(x => x.Accounts).ThenInclude(x =>x.Student)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (order is null)
            return Errors.CardOrder.NotFound;

        return new CardOrderDetailsResult(
            order.Id,
            order.TeacherId,
            order.TeacherName,
            order.CreatedAt,
            order.CompletedAt,
            order.CancelledAt,
            order.AcceptedBy,
            order.CancelledBy,
            order.Accounts.Count,
            order.Status,
            order.Accounts.Select(x => new ShortAccount(x.Id, x.Student.Name)).ToList());
    }
}