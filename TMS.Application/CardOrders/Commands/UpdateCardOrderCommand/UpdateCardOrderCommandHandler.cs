using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TMS.Application.CardOrders.Commands.Create;
using TMS.Application.Common.Services;
using TMS.Application.Common.Variables;
using TMS.Domain.Admins;
using TMS.Domain.Cards;
using TMS.Domain.Common.Errors;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.CardOrders.Commands.UpdateCardOrderCommand;

public class UpdateCardOrderCommandHandler : IRequestHandler<UpdateCardOrderCommand, ErrorOr<CardOrderResult>>
{
    private readonly IClaimsReader _claimsReader;
    private readonly ICardOrderRepository _cardOrderRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITeacherHelper _teacherHelper;

    public UpdateCardOrderCommandHandler(IClaimsReader claimsReader, ICardOrderRepository cardOrderRepository,
        IDateTimeProvider dateTimeProvider, ITeacherHelper teacherHelper)
    {
        _claimsReader = claimsReader;
        _cardOrderRepository = cardOrderRepository;
        _dateTimeProvider = dateTimeProvider;
        _teacherHelper = teacherHelper;
    }

    public async Task<ErrorOr<CardOrderResult>> Handle(UpdateCardOrderCommand request,
        CancellationToken cancellationToken)
    {
        var roles = _claimsReader.GetRoles();
        if (roles.Contains(Roles.Admin.Role))
        {
            var order = _cardOrderRepository.GetQueryable().Include(x => x.AccountIds)
                .FirstOrDefault(x => x.Id == request.Id);
            if (order == null)
                return Errors.CardOrder.NotFound;
            var adminId = _claimsReader.GetByClaimType(CustomClaimTypes.Id);
            if (adminId is null)
                return Errors.Auth.InvalidCredentials;
            switch (order.Status)
            {
                case CardOrderStatus.Processing:
                    if (request.Status != CardOrderStatus.Completed)
                        return Errors.CardOrder.OrderIsNotOnProcessing;

                    order.UpdateStatus(AdminId.Create(adminId), request.Status, _dateTimeProvider.Now);

                    break;

                case CardOrderStatus.Cancelled:
                    return Errors.CardOrder.OrderAlreadyRejected;

                case CardOrderStatus.Pending:
                    if (request.Status != CardOrderStatus.Completed)
                        return Errors.CardOrder.CanNotCompletePendingOrder;
                    order.UpdateStatus(AdminId.Create(adminId), request.Status, _dateTimeProvider.Now);
                    break;
            }

            return CardOrderResult.From(order);
        }
        else
        {
            var teacherId = _teacherHelper.GetTeacherId();
            var order = _cardOrderRepository.GetQueryable().Include(x => x.AccountIds)
                .FirstOrDefault(x => x.Id == request.Id && x.TeacherId == teacherId);
            if (order == null)
                return Errors.CardOrder.NotFound;
            
            if (order.Status != CardOrderStatus.Pending)
                return Errors.CardOrder.OrderIsNotOnPending;
            
            if (request.Ids is not null)
            {
                order.UpdateAccounts(request.Ids);
            }

            order.Status = request.Status;
            return CardOrderResult.From(order);
            
        }
    }
}