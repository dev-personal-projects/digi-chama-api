//OTP generation and validation
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Auth.Core.Interfaces;
using Microsoft.Extensions.Options;
using Auth.Core.Settings;

namespace Auth.Core.Services
{
    public class OtpService : IOtpService
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly OtpSettings _otpSettings;

        public OtpService(
            IEmailService emailService,
            ISmsService smsService,
            IOptions<OtpSettings> otpSettings)
        {
            _emailService = emailService;
            _smsService = smsService;
            _otpSettings = otpSettings.Value;
        }

        /// <summary>
        /// Generates and sends OTP via both email and SMS
        /// </summary>
        public async Task<(string otp, DateTime expiry)> GenerateAndSendOtpAsync(string email, string phoneNumber)
        {
            // Generate OTP
            string otp = GenerateOtp();
            DateTime expiry = DateTime.UtcNow.AddMinutes(_otpSettings.ExpiryMinutes);

            // Send via both channels concurrently
            var emailTask = SendOtpEmail(email, otp);
            var smsTask = SendOtpSms(phoneNumber, otp);

            await Task.WhenAll(emailTask, smsTask);

            return (otp, expiry);
        }

        /// <summary>
        /// Validates OTP and checks expiry
        /// </summary>
        public bool ValidateOtp(string providedOtp, string storedOtp, DateTime expiryTime)
        {
            if (string.IsNullOrEmpty(providedOtp) || string.IsNullOrEmpty(storedOtp))
                return false;

            if (DateTime.UtcNow > expiryTime)
                return false;

            // Use constant-time comparison to prevent timing attacks
            return CryptographicOperations.FixedTimeEquals(
                System.Text.Encoding.UTF8.GetBytes(providedOtp),
                System.Text.Encoding.UTF8.GetBytes(storedOtp));
        }

        private string GenerateOtp()
        {
            // Generate cryptographically secure random number
            using var rng = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[4];
            rng.GetBytes(randomBytes);
            
            // Convert to 6-digit number
            int otpNumber = Math.Abs(BitConverter.ToInt32(randomBytes, 0) % 1000000);
            return otpNumber.ToString("D6");
        }

        private async Task SendOtpEmail(string email, string otp)
        {
            var subject = "Your Verification Code";
            var body = $@"
                <h2>Verification Code</h2>
                <p>Your verification code is: <strong>{otp}</strong></p>
                <p>This code will expire in {_otpSettings.ExpiryMinutes} minutes.</p>
                <p>If you didn't request this code, please ignore this email.</p>";

            await _emailService.SendEmailAsync(email, subject, body);
        }

        private async Task SendOtpSms(string phoneNumber, string otp)
        {
            var message = $"Your verification code is: {otp}. Valid for {_otpSettings.ExpiryMinutes} minutes.";
            await _smsService.SendSmsAsync(phoneNumber, message);
        }
    }
}