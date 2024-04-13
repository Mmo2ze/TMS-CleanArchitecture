using TMS.Domain.Students;

namespace TMS.Domain.Common.Repositories;

public interface IStudentRepository
{
	Task<bool> IsStudent(string phone);
	Task<Student?> GetStudentByPhone(string phone);
}