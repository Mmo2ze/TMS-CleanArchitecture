using TMS.Domain.Admins;
using TMS.Domain.Assistants;
using TMS.Domain.Parents;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Domain.RefreshTokens;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public Guid TokenId { get; set; } 
    public DateTime Expires { get; set; }
    public bool IsActive => DateTime.UtcNow < Expires;
    public DateTime CreatedOn { get; set; }
    public DateTime? RevokedOn { get; set; }
    public TeacherId? TeacherId { get; set; }
    public AdminId? AdminId { get; set; }
    public StudentId? StudentId { get; set; }
    public ParentId? ParentId { get; set; }
    public AssistantId? AssistantId { get; set; }

    
}