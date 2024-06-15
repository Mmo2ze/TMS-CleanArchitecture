namespace TMS.Consumer.MessageTemplates;

public static partial class MsgTemplate
{
    public static  class Admin
    {
        public static string TeacherPhoneNotHaveWhatsapp(string teacherName,string teacherPhone)
        {
            return $"المعلم {teacherName} لا يملك واتساب، يرجى التواصل معه على الرقم {teacherPhone}";
        }
        
        
    }
}