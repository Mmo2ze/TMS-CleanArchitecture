using ErrorOr;
using MediatR;
using TMS.Application.Common.Services;
using TMS.Domain.Cards;
using TMS.Domain.Common.Repositories;

namespace TMS.Application.CardOrders.Commands.Create;

public class CreateCardOrderHandler : IRequestHandler<CreateCardOrderCommand, ErrorOr<CardOrderResult>>
{
    private readonly ITeacherHelper _teacherHelper;
    private readonly ITeacherRepository _teacherRepository;
    private readonly ICardOrderRepository _cardOrderRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAccountRepository _accountRepository;

    public CreateCardOrderHandler(ITeacherRepository teacherRepository, ICardOrderRepository cardOrderRepository,
        ITeacherHelper teacherHelper, IDateTimeProvider dateTimeProvider, IAccountRepository accountRepository)
    {
        _teacherRepository = teacherRepository;
        _cardOrderRepository = cardOrderRepository;
        _teacherHelper = teacherHelper;
        _dateTimeProvider = dateTimeProvider;
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<CardOrderResult>> Handle(CreateCardOrderCommand request,
        CancellationToken cancellationToken)
    {
        var teacherName = _teacherRepository.GetQueryable()
            .Select(x => new { x.Id, x.Name })
            .First(x => x.Id == _teacherHelper.GetTeacherId()).Name;

        var accounts = _accountRepository.WhereQueryable(x => request.AccountIds.Contains(x.Id)).ToList();
        var cardOrder = CardOrder.Create(accounts, _teacherHelper.GetTeacherId(), _dateTimeProvider.Now,
            teacherName);
        _cardOrderRepository.Add(cardOrder);
        return CardOrderResult.From(cardOrder);
    }
}