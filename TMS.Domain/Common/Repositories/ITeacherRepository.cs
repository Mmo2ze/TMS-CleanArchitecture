using System.Linq.Expressions;
using TMS.Domain.Teachers;

namespace TMS.Domain.Common.Repositories;

public interface ITeacherRepository
{
    Task<bool> IsTeacher(string phone,  CancellationToken cancellationToken = default);
    Task<Teacher?> GetByPhone(string phone, CancellationToken cancellationToken = default);

    Task<Teacher?> GetTeacher(Expression<Func<Teacher, bool>> predicate, CancellationToken cancellationToken = default);
    Task<bool> Any(Expression<Func<Teacher, bool>> predicate ,CancellationToken cancellationToken = default);

    Task AddAsync(Teacher teacher, CancellationToken cancellationToken = default);
    Task<List<Teacher>> GetTeachers(int requestPage, int requestPageSize, CancellationToken cancellationToken = default);
    Task UpdateTeacher(Teacher teacher, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}