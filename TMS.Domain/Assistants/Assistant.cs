using TMS.Domain.Common.Models;
using TMS.Domain.Teachers;

namespace TMS.Domain.Assistants;

public class Assistant:User
{	
	public AssistantId Id { get; private set; }

	public TeacherId TeacherId { get; private set; }
	private Assistant(AssistantId id, string name, string? email, string phone, TeacherId teacherId)
	{
		Id = id;
		Name = name;
		Email = email;
		Phone = phone;
		TeacherId = teacherId;
	}
	public static Assistant Create(string name,  string phone, TeacherId teacherId,string? email = null)
	{
		return new Assistant(AssistantId.CreateUnique(), name, email, phone, teacherId);
	}
	
}