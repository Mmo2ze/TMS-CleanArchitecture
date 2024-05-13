using System.Linq.Expressions;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Teachers;

namespace TMS.Infrastructure.Persistence.Repositories;

public class TeacherRepository : Repository<Teacher,TeacherId>,ITeacherRepository
{

    public TeacherRepository(MainContext dbContext):base(dbContext)
    {
    }

    public Task<bool> IsTeacher(string phone,  CancellationToken cancellationToken = default)
    {
        return DbContext.Teachers.AnyAsync(teacher => teacher.Phone == phone, cancellationToken);
    }

    public Task<Teacher?> GetByPhone(string phone, CancellationToken cancellationToken = default)
    {
        return DbContext.Teachers.FirstOrDefaultAsync(teacher => teacher.Phone == phone,cancellationToken);
    }

    public Task<Teacher?> GetTeacher(Expression<Func<Teacher, bool>> predicate ,CancellationToken cancellationToken = default)
    {
        return DbContext.Teachers.FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken);
    }

    public Task<bool> Any(Expression<Func<Teacher, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return DbContext.Teachers.AnyAsync(predicate, cancellationToken: cancellationToken);
    }

    public async Task AddAsync(Teacher teacher, CancellationToken cancellationToken = default)
    {
            await DbContext.AddAsync(teacher, cancellationToken);
    }

    public async Task<IQueryable<Teacher>> GetTeachers(int requestPage, int requestPageSize, CancellationToken cancellationToken = default)
    {
        var teachers = await GetTeachers(cancellationToken);
        return teachers.Skip((requestPage - 1) * requestPageSize).Take(requestPageSize);
          
    }

    public async Task<IQueryable<Teacher>> GetTeachers(CancellationToken cancellationToken = default)
    {
        return DbContext.Teachers
            .OrderBy(x => x.JoinDate);
    }


    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Teacher?> GetByIdAsync(TeacherId requestTeacherId, CancellationToken cancellationToken)
    {
        return DbContext.Teachers.FirstOrDefaultAsync(teacher => teacher.Id == requestTeacherId, cancellationToken);
    }

    public async Task<Error?> DeleteAsync(TeacherId requestId, CancellationToken cancellationToken)
    {
        var teacher = await DbContext.Teachers.FirstOrDefaultAsync(teacher => teacher.Id == requestId, cancellationToken);
        if (teacher == null)
            return Error.NotFound();
        DbContext.Teachers.Remove(teacher);
        return null;
    }


    public Task UpdateTeacher(Teacher teacher, CancellationToken cancellationToken)
    {
        DbContext.Update(teacher);
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}