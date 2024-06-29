namespace TMS.Application.Common.Services;

public interface IDateTimeProvider
{
	DateTime Now { get; }
	DateOnly Today { get; }
	TimeOnly TimeNow { get; }
}