namespace Auth.Core.Settings
{
    /// <summary>
    /// Settings for Twilio SMS service
    /// </summary>
    public class SmsSettings
    {
        /// <summary>
        /// Twilio Account SID
        /// </summary>
        public string AccountSid { get; set; } = string.Empty;

        /// <summary>
        /// Twilio Auth Token
        /// </summary>
        public string AuthToken { get; set; } = string.Empty;

        /// <summary>
        /// Twilio Phone Number to send from
        /// </summary>
        public string FromNumber { get; set; } = string.Empty;

        /// <summary>
        /// Maximum retries for failed SMS attempts
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// Delay between retries in seconds
        /// </summary>
        public int RetryDelaySeconds { get; set; } = 2;
    }
}