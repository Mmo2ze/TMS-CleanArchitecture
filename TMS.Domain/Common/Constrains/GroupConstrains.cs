namespace TMS.Domain.Common.Constrains;

public static partial class Constrains
{
    public static class Group
    {
        public static Length Name => new(1, 50);
    }

}