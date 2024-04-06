using TMS.Domain.Students;

namespace TMS.Domain.Parents;

public class Parent
{
	
	public ParentId Id { get; private set; } = null!;
	public string Name { get; private set; } = null!;
	public string? Email { get; private set; }
	public string? Phone { get; private set; }
	public Gender Gender { get;private set; }

	private Parent(ParentId id, string name, Gender gender, string? email = null, string? phone = null)
	{
		Id = id;
		Name = name;
		Email = email;
		Gender = gender;
		Phone = phone;
	}

	public static Parent Create(string name, Gender gender, string? email = null, string? phone = null)
	{
		return new Parent(ParentId.CreateUnique(), name,gender, email, phone);
	}
}