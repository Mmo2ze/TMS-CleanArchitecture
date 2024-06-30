using TMS.Domain.Cards;
using TMS.Domain.Common.Repositories;

namespace TMS.Infrastructure.Persistence.Repositories;

public class CardOrderRepository:Repository<CardOrder,CardOrderId>,ICardOrderRepository
{
    public CardOrderRepository(MainContext dbContext) : base(dbContext)
    {
    }
}