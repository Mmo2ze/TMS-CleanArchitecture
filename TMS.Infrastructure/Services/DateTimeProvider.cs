using TMS.Application.Common.Services;

namespace TMS.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
	public DateTime Now => DateTime.Now;
	public DateOnly Today => DateOnly.FromDateTime(Now);
}