namespace TMS.Domain.Admins;

public class Admin:User<AdminId>
{

	private Admin(AdminId id, string name, string email, string phone):base(id)
	{
		Name = name;
		Email = email;
		Phone = phone;
	}
	public static Admin Create(string name, string email, string phone)
	{
		return new Admin(AdminId.CreateUnique(), name, email, phone);
	}
}