using TMS.Domain.Teachers;

namespace TMS.Domain.Assistants;

public class Assistant:User<AssistantId>
{

	private Assistant():base(AssistantId.CreateUnique())
	{
		
	}
	public string RolesString { get; private set; } 
	public List<AssistantRole> Roles =>   string.IsNullOrWhiteSpace(RolesString)? []: RolesString.Split(',').Select(Enum.Parse<AssistantRole>).ToList(); 
	public TeacherId TeacherId { get; private set; }
	
	private Assistant(AssistantId id, string name, string? email, string phone, TeacherId teacherId):base(id)
	{
		Name = name;
		Email = email;
		Phone = phone;
		TeacherId = teacherId;
	}
	public static Assistant Create(string name,  string phone, TeacherId teacherId,string? email = null)
	{
		return new Assistant(AssistantId.CreateUnique(), name, email, phone, teacherId);
	}
	
	
	public void Update(string name, string phone, string? email)
	{
		Name = name;
		Phone = phone;
		Email = email;
	}
	
	public void UpdateRoles(List<AssistantRole> roles)
	{
		RolesString = string.Join(",", roles.Select(x => x.ToString()));
	}
	
	public void AddRole(AssistantRole role)
	{
		var roles = Roles;
		roles.Add(role);
		RolesString = string.Join(",", roles.Select(x => x.ToString()));
	}
	
	public void RemoveRole(AssistantRole role)
	{
		var roles = Roles;
		roles.Remove(role);
		RolesString = string.Join(",", roles.Select(x => x.ToString()));
	}
	
	
	
}