using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Auth.Core.Interfaces;
using Auth.Core.Settings;

namespace Auth.Core.Services
{
    /// <summary>
    /// Implementation of SMS service using Twilio
    /// </summary>
    public class SmsService : ISmsService
    {
        private readonly SmsSettings _settings;
        private readonly ILogger<SmsService> _logger;

        public SmsService(
            IOptions<SmsSettings> settings,
            ILogger<SmsService> logger)
        {
            _settings = settings.Value;
            _logger = logger;

            // Initialize Twilio client with retry policy
            try
            {
                TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
                _logger.LogInformation("Twilio client initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initialize Twilio client");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message))
            {
                _logger.LogError("Phone number or message content is empty");
                return false;
            }

            try
            {
                // Validate and format phone number
                var to = new PhoneNumber(FormatPhoneNumber(phoneNumber));
                var from = new PhoneNumber(_settings.FromNumber);

                var messageResource = await MessageResource.CreateAsync(
                    to: to,
                    from: from,
                    body: message
                );

                if (messageResource.Status == MessageResource.StatusEnum.Sent ||
                    messageResource.Status == MessageResource.StatusEnum.Queued)
                {
                    _logger.LogInformation(
                        "SMS sent successfully. SID: {MessageSid}, Status: {Status}, To: {PhoneNumber}", 
                        messageResource.Sid, 
                        messageResource.Status, 
                        phoneNumber);
                    return true;
                }

                _logger.LogError(
                    "Failed to send SMS. Status: {Status}, To: {PhoneNumber}", 
                    messageResource.Status, 
                    phoneNumber);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS to {PhoneNumber}", phoneNumber);
                return false;
            }
        }

        /// <summary>
        /// Formats phone number to E.164 format
        /// </summary>
        private string FormatPhoneNumber(string phoneNumber)
        {
            // Remove any non-digit characters
            var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Ensure number starts with +
            if (!digitsOnly.StartsWith("+"))
            {
                digitsOnly = "+" + digitsOnly;
            }

            return digitsOnly;
        }
    }
}