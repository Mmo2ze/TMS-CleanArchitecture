namespace TMS.Consumer.MessageTemplates;

public static  partial class MsgTemplate
{
    public static  class Teacher
    {
        public static string SubscriptionUpdated(string teacherName, DateOnly endOfSubscription)
        {
            return $"أهلا {teacherName}، تم تجديد اشتراكك لغاية {endOfSubscription:dd/MM/yyyy}، شكرا لثقتك بنا";
        }

        public static string WelcomeTeacher(string teacherName,DateOnly endOfSubscription)
        {
            return "أهلا بك " + teacherName + " في تطبيقنا، تم تفعيل اشتراكك لغاية " + endOfSubscription.ToString("dd/MM/yyyy") + "، نتمنى لك تجربة ممتعة";
        }
        
    }

    public class Attendance
    {
        public static string AbsenceRecorded(string parentName, string studentShortName)
        {
            return $"أهلا استاذ  {parentName}، تم تسجيل غياب  {studentShortName} في الحصة الخاصة بـ ";
        }
    }
}