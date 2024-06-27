using TMS.Domain.Common.Repositories;
using TMS.Domain.Holidays;

namespace TMS.Infrastructure.Persistence.Repositories;

public class HolidayRepository:Repository<Holiday,HolidayId>,IHolidayRepository
{
    public HolidayRepository(MainContext dbContext) : base(dbContext)
    {
    }
}