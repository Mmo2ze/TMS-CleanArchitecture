using Microsoft.EntityFrameworkCore;
using TMS.Domain.Common.Repositories;
using TMS.Domain.Groups;
using TMS.Domain.Students;

namespace TMS.Infrastructure.Persistence.Repositories;

public class StudentRepository : Repository<Student, StudentId>, IStudentRepository
{
    public StudentRepository(MainContext dbContext) : base(dbContext)
    {
    }

    public Task<bool> IsStudent(string phone)
    {
        return DbContext.Students.AnyAsync(s => s.Phone == phone);
    }

    public Task<Student?> GetStudentByPhone(string phone)
    {
        return DbContext.Students.FirstOrDefaultAsync(s => s.Phone == phone);
    }



    
}