using TMS.Domain.Admins;
using TMS.Domain.Teachers;

namespace TMS.Domain.Assistants;

public class Assistant
{	
	public AssistantId Id { get; private set; }
	public string Name { get; private set; }
	public string Email { get; private set; }
	public string Phone { get; private set; }
	public TeacherId TeacherId { get; private set; }
	private Assistant(AssistantId id, string name, string email, string phone, TeacherId teacherId)
	{
		Id = id;
		Name = name;
		Email = email;
		Phone = phone;
		TeacherId = teacherId;
	}
	public static Assistant Create(string name, string email, string phone, TeacherId teacherId)
	{
		return new Assistant(AssistantId.CreateUnique(), name, email, phone, teacherId);
	}
	
}