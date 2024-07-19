namespace TMS.Domain.Teachers;

public enum Subject
{
    Maths,
    Arabic,
    English,
    French,
    Chemistry,
    Physics,
    Biology,
    Geography,
    History,
    German,
    Geology,
    ScotiaStudies,
}


public static class MyClass
{

    public static string ToArabic(this Subject subject)
    {
        return subject switch
        {
            Subject.Maths => "الرياضيات",
            Subject.Arabic => "العربي",
            Subject.English => "الإنجليزي",
            Subject.French => "الفرنسي",
            Subject.Chemistry => "الكيمياء",
            Subject.Physics => "الفيزياء",
            Subject.Biology => "الأحياء",
            Subject.Geography => "الجغرافيا",
            Subject.History => "التاريخ",
            Subject.German => "الألماني",
            Subject.Geology => "الجيولوجيا",
            Subject.ScotiaStudies => "الدراسات الاجتماعية",
            _ => throw new ArgumentOutOfRangeException(nameof(subject), subject, null),
        };
    }
}