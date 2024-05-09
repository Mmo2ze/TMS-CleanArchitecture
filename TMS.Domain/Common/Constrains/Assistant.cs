namespace TMS.Domain.Common.Constrains;

public static partial class Constrains
{
    public static class Assistant
    {
        public static Length Name => new(1, 26);
    }
}