namespace TMS.Domain.Common.Constrains;

public static partial class Constrains
{
    
    public static Length Email => new(7,126);
    public static Length Phone => new(9, 16);
}