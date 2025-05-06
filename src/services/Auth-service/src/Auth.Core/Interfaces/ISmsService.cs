namespace Auth.Core.Interfaces
{
    /// <summary>
    /// Interface for SMS operations
    /// </summary>
    public interface ISmsService
    {
        /// <summary>
        /// Sends an SMS message asynchronously
        /// </summary>
        /// <param name="phoneNumber">Recipient's phone number in E.164 format</param>
        /// <param name="message">Message content</param>
        /// <returns>True if sent successfully, false otherwise</returns>
        Task<bool> SendSmsAsync(string phoneNumber, string message);
    }
}