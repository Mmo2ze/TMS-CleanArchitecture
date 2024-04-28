using TMS.Domain.Sessions;
using TMS.Domain.Students;
using TMS.Domain.Teachers;

namespace TMS.Domain.Classes;

public class Class
{
    private readonly List<Student> _students = [];
    private readonly List<Session> _sessions = [];
    public ClassId Id { get; private set; }
    public string Name { get; private set; }
    public Grade Grade { get; private set; }
    public TeacherId TeacherId { get; private set; }
    
    public IReadOnlyList<Student> Students => _students.AsReadOnly();
    public IReadOnlyList<Session> Sessions => _sessions.AsReadOnly();
    

    private Class(ClassId id, string name, Grade grade, TeacherId teacherId)
    {
        Id = id;
        Name = name;
        Grade = grade;
        TeacherId = teacherId;
    }

    public static Class Create(string name, Grade grade, TeacherId teacherId)
    {
        return new Class(ClassId.CreateUnique(), name, grade, teacherId);
    }
}