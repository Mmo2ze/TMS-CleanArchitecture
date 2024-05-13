namespace TMS.Domain.Common.Models;

public class User<TId>:Aggregate<TId>
{
    protected User(TId id):base(id)
    {
        
    }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public string Name { get; protected set; } = string.Empty;
    public virtual string Phone { get; protected set; } = string.Empty;
    public  string? Email { get; protected set; } = string.Empty;
    
}