namespace Auth.Core.Settings
{
    /// <summary>
    /// Settings for SendGrid email service
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// SendGrid API Key
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Sender email address
        /// </summary>
        public string FromEmail { get; set; } = string.Empty;

        /// <summary>
        /// Sender name
        /// </summary>
        public string FromName { get; set; } = string.Empty;

        /// <summary>
        /// SendGrid template ID for OTP emails
        /// </summary>
        public string OtpTemplateId { get; set; } = string.Empty;

        /// <summary>
        /// Maximum retries for failed email attempts
        /// </summary>
        public int MaxRetries { get; set; } = 3;

        /// <summary>
        /// Reply-to email address
        /// </summary>
        public string ReplyToEmail { get; set; } = string.Empty;
    }
}