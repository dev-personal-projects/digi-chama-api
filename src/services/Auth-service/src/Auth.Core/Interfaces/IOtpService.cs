namespace Auth.Core.Interfaces
{
    public interface IOtpService
    {
        Task<(string otp, DateTime expiry)> GenerateAndSendOtpAsync(string email, string phoneNumber);
        bool ValidateOtp(string providedOtp, string storedOtp, DateTime expiryTime);

    }
}