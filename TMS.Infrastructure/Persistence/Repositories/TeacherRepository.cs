using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Persistence;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Repositories;

public class TeacherRepository:ITeacherRepository
{
	private readonly MainContext _dbContext;

	public TeacherRepository(MainContext dbContext)
	{
		_dbContext = dbContext;
	}

	public  Task<bool> IsTeacher(string phone)
	{
		return _dbContext.Teachers.AnyAsync(teacher => teacher.Phone == phone);
	}

	public  Task<Teacher?> GetTeacherByPhone(string phone)
	{
		return _dbContext.Teachers.FirstOrDefaultAsync(teacher => teacher.Phone == phone);
	}

	public void Add(Teacher teacher)
	{
		_dbContext.Add(teacher);
		_dbContext.SaveChanges();
	}
}