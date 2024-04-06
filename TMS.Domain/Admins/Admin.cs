namespace TMS.Domain.Admins;

public class Admin
{
	public AdminId Id { get; set; } 
	public string Name { get; set; }
	public string Email { get; set; }
	public string Phone { get; set; }
	private Admin(AdminId id, string name, string email, string phone)
	{
		Id = id;
		Name = name;
		Email = email;
		Phone = phone;
	}
	public static Admin Create(string name, string email, string phone)
	{
		return new Admin(AdminId.CreateUnique(), name, email, phone);
	}
}