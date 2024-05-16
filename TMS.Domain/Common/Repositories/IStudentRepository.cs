using TMS.Domain.Groups;
using TMS.Domain.Students;

namespace TMS.Domain.Common.Repositories;

public interface IStudentRepository:IRepository<Student,StudentId>
{
	Task<bool> IsStudent(string phone);
	Task<Student?> GetStudentByPhone(string phone);

}