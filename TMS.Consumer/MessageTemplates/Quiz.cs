namespace TMS.Consumer.MessageTemplates;

public static partial class MsgTemplate
{
    public static class Quiz
    {
        public static string Created(double degree, double maxDegree, string parentName, string studentName,
            string AddedByName)
        {
            return
                $"تم إنشاء اختبار جديد بنجاح بدرجة {degree} من أصل {maxDegree} للطالب {studentName} من قبل {AddedByName}";
        }
        
        public static string Updated(double degree, double maxDegree, string parentName, string studentName,
            string AddedByName)
        {
            return
                $"تم تحديث اختبار بدرجة {degree} من أصل {maxDegree} للطالب {studentName} من قبل {AddedByName}";
        }
        
        public static string Removed(string parentName, string studentName)
        {
            return
                $"تم حذف اختبار للطالب {studentName} ";
        }
    }
}