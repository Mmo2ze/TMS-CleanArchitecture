namespace TMS.Domain.Common.Constrains;

public static partial class Constrains
{
    public class Teacher
    {
        public static Length Name => new(4, 26);
    }
}