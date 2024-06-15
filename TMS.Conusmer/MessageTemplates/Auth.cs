namespace TMS.Consumer.MessageTemplates;

public static partial class MsgTemplate
{
    public static class Auth
    {
        public static string VerificationCodeCreated(string code)
        {
            return $"كود التحقق الخاص بك هو {code}";
        }
    }
}