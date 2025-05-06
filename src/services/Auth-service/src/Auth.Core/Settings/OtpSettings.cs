namespace Auth.Core.Settings
{
    public class OtpSettings
    {
        public int ExpiryMinutes{get;set;} = 5;
        public int MaxAttempts{get;set;} = 3;
        public int CodeLength {get;set; } = 6;

    }

}