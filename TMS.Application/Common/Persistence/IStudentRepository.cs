using TMS.Domain.Students;

namespace TMS.Application.Common.Persistence;

public interface IStudentRepository
{
	Task<bool> IsStudent(string phone);
	Task<Student?> GetStudentByPhone(string phone);
}