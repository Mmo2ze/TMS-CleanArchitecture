using TMS.Domain.Teachers;

namespace TMS.Domain.Students;

public class Attendance
{
	private Attendance()
	{
		
	}
	private Attendance(StudentId studentId, DateOnly date, TeacherId teacherId)
	{
		StudentId = studentId;
		Date = date;
		TeacherId = teacherId;
	}
	public StudentId StudentId { get; private set; }
	public TeacherId TeacherId { get; private set; }
	public DateOnly Date { get; private set; }
	public AttendanceStatus Status { get; private set; }
}