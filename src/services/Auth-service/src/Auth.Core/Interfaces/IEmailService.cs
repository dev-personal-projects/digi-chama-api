namespace Auth.Core.Interfaces
{
     /// <summary>
    /// Interface for email operations using SendGrid
    /// </summary>
    public interface IEmailService
    {
                /// <summary>
        /// Sends an email asynchronously
        /// </summary>
        /// <param name="to">Recipient email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="htmlContent">HTML content of the email</param>
        /// <returns>True if sent successfully, false otherwise</returns>
        Task<bool> SendEmailAsync(string to, string subject, string htmlContent);
                /// <summary>
        /// Sends a templated email using SendGrid dynamic templates
        /// </summary>
        /// <param name="to">Recipient email address</param>
        /// <param name="templateId">SendGrid template ID</param>
        /// <param name="dynamicData">Dynamic template data</param>
        /// <returns>True if sent successfully, false otherwise</returns>
        Task<bool> SendTemplatedEmailAsync(string to, string templateId, object dynamicData);

    }

}