namespace TMS.Domain.Holidays;

public record HolidayId(string Value) : ValueObjectId<HolidayId>(Value)
{
    public HolidayId() : this(Ulid.NewUlid().ToString())
    {
    }
}