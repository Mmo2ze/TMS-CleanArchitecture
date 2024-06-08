using TMS.Domain.Common.Enums;
using TMS.Domain.Students;

namespace TMS.Domain.Parents;

public class Parent:User<ParentId>
{
	
	private readonly List<Account.Account> _children = [];
	public ParentId Id { get; private set; }
	public override string? Phone { get; protected set; }
	public Gender Gender { get;private set; }
	public IReadOnlyList<Account.Account> Children => _children.AsReadOnly();

	private Parent(ParentId id, string name, Gender gender, string? email = null, string? phone = null):base(id)
	{
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