namespace TMS.Domain.Common.Models;

public class User
{
    public string Name { get; protected set; } = string.Empty;
    public virtual string Phone { get; protected set; } = string.Empty;
    public  string? Email { get; protected set; } = string.Empty;
    
}