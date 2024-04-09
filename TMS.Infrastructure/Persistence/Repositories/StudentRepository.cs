using Microsoft.EntityFrameworkCore;
using TMS.Application.Common.Persistence;
using TMS.Domain.Students;

namespace TMS.Infrastructure.Persistence.Repositories;

public class StudentRepository:IStudentRepository
{
	private readonly MainContext _dbContext;

	public StudentRepository(MainContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task<bool> IsStudent(string phone)
	{
		return _dbContext.Students.AnyAsync(s => s.Phone == phone);
	}

	public Task<Student?> GetStudentByPhone(string phone)
	{
		return _dbContext.Students.FirstOrDefaultAsync(s => s.Phone == phone);
	}
}