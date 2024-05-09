namespace TMS.Domain.Common.Constrains;

public static partial class Constrains
{
    public static class Admin
    {
        public static Length Name => new(1, 26);
    }
    
   
}