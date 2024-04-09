using TMS.Domain.Teachers;

namespace TMS.Application.Common.Persistence;

public interface ITeacherRepository
{
	Task<bool> IsTeacher(string phone);
	Task<Teacher?> GetTeacherByPhone(string phone);


	void Add(Teacher teacher);
}