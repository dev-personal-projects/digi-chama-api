using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Auth.Core.Interfaces;
using Auth.Core.Settings;
using System.Text.RegularExpressions;

namespace Auth.Core.Services
{
    /// <summary>
    /// Implementation of email service using SendGrid
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly SendGridClient _client;
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailSettings> settings,
            ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _client = new SendGridClient(_settings.ApiKey);
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<bool> SendEmailAsync(string to, string subject, string htmlContent)
        {
            try
            {
                var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
                var toAddress = new EmailAddress(to);
                var plainTextContent = StripHtml(htmlContent);

                var msg = MailHelper.CreateSingleEmail(
                    from,
                    toAddress,
                    subject,
                    plainTextContent,
                    htmlContent
                );

                if (!string.IsNullOrEmpty(_settings.ReplyToEmail))
                {
                    msg.ReplyTo = new EmailAddress(_settings.ReplyToEmail);
                }

                var response = await _client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Email sent successfully to {Email}", to);
                    return true;
                }

                _logger.LogError("Failed to send email to {Email}. Status: {Status}",
                    to, response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Email}", to);
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> SendTemplatedEmailAsync(string to, string templateId, object dynamicData)
        {
            try
            {
                var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
                var toAddress = new EmailAddress(to);

                var msg = MailHelper.CreateSingleTemplateEmail(
                    from,
                    toAddress,
                    templateId,
                    dynamicData
                );

                var response = await _client.SendEmailAsync(msg);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation(
                        "Templated email sent successfully to {Email} using template {TemplateId}",
                        to, templateId);
                    return true;
                }

                _logger.LogError(
                    "Failed to send templated email to {Email}. Status: {Status}",
                    to, response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error sending templated email to {Email} using template {TemplateId}",
                    to, templateId);
                return false;
            }
        }

        private string StripHtml(string html)
        {
            return Regex.Replace(html, "<.*?>", string.Empty);
        }
    }
}