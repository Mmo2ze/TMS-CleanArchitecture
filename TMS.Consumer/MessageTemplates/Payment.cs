using TMS.Domain.Common.Enums;

namespace TMS.Consumer.MessageTemplates;

public static partial class MsgTemplate
{
    public static class Payment
    {
        public static string PaymentRemoved(string parentName, string studentName, decimal amount, DateOnly date,
            Gender studentGender)
        {
            var month = date.Month;
            switch (studentGender)
            {
                case Gender.Male:
                    return $"استاذ {parentName} " +
                           $"تم حذف مصاريف شهر {month} للطالب {studentName} بمبلغ {amount} جنيه";
                case Gender.Female:
                    return $"استاذة {parentName} " +
                           $"تم حذف مصاريف شهر {month} للطالبة {studentName} بمبلغ {amount} جنيه";
                default:
                    throw new ArgumentOutOfRangeException(nameof(studentGender), studentGender, "Invalid");
            }
        }

        public static string PaymentAdded(string parentName, string studentName, decimal amount, DateOnly date,
            Gender studentGender)
        {
            var month = date.Month;
            switch (studentGender)
            {
                case Gender.Male:
                    return $"استاذ {parentName} " +
                           $"تم اضافة مصاريف شهر {month} للطالب {studentName} بمبلغ {amount} جنيه";
                case Gender.Female:
                    return $"استاذة {parentName} " +
                           $"تم اضافة مصاريف شهر {month} للطالبة {studentName} بمبلغ {amount} جنيه";
                default:
                    throw new ArgumentOutOfRangeException(nameof(studentGender), studentGender, "Invalid");
            }
        }

        public static string PaymentUpdated(string parentName, string studentName, decimal amount, DateOnly date,
            Gender studentGender)
        {
            var month = date.Month;
            switch (studentGender)
            {
                case Gender.Male:
                    return $" استاذ {parentName} " +
                           $"تم تعديل مصاريف شهر {month} للطالب {studentName} بمبلغ {amount} جنيه";
                case Gender.Female:
                    return $" استاذة {parentName} " +
                           $"تم تعديل مصاريف شهر {month} للطالبة {studentName} بمبلغ {amount} جنيه";
                default:
                    throw new ArgumentOutOfRangeException(nameof(studentGender), studentGender, "Invalid");
            }
        }
    }
}