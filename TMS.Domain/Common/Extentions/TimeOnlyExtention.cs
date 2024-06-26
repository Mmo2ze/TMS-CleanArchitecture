namespace TMS.Domain.Common.Extentions;

public static class TimeOnlyExtensions
{
    public static TimeOnly SetSecondsToZero(this TimeOnly time)
    {
        return new TimeOnly(time.Hour, time.Minute, time.Second, 0); // seconds and fraction-of-second are set to 0
    }
}