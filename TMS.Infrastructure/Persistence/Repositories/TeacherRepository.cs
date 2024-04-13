using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly MainContext _dbContext;

    public TeacherRepository(MainContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> IsTeacher(string phone,  CancellationToken cancellationToken = default)
    {
        return _dbContext.Teachers.AnyAsync(teacher => teacher.Phone == phone, cancellationToken);
    }

    public Task<Teacher?> GetByPhone(string phone, CancellationToken cancellationToken = default)
    {
        return _dbContext.Teachers.FirstOrDefaultAsync(teacher => teacher.Phone == phone,cancellationToken);
    }

    public Task<Teacher?> GetTeacher(Expression<Func<Teacher, bool>> predicate ,CancellationToken cancellationToken = default)
    {
        return _dbContext.Teachers.FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public Task<bool> Any(Expression<Func<Teacher, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _dbContext.Teachers.AnyAsync(predicate, cancellationToken: cancellationToken);
    }

    public async Task Add(Teacher teacher, CancellationToken cancellationToken = default)
    {
            await _dbContext.AddAsync(teacher,cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<List<Teacher>> GetTeachers(int requestPage, int requestPageSize, CancellationToken cancellationToken = default)
    {
        return _dbContext.Teachers
            .Skip((requestPage - 1) * requestPageSize)
            .Take(requestPageSize)
            .OrderBy(x => x.JoinDate)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public Task UpdateTeacher(Teacher teacher, CancellationToken cancellationToken)
    {
        _dbContext.Update(teacher);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}