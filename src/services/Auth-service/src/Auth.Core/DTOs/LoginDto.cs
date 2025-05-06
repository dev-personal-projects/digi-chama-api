//login Dto for login request
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Auth.Common.Enums;


namespace Auth.Core.DTOs
{
    /// <summary>
    /// Data transfer object for user login requests.
    /// Supports multiple authentication methods including biometrics.
    /// </summary>
    /// 
    public class LoginDto
    {
        /// <summary>
        /// Email address for email-based login
        /// </summary>
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        /// <summary>
        /// Phone number for phone-based login
        /// </summary>
        [Phone(ErrorMessage = "Invalid phone number format")]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        /// <summary>
        /// Password for traditional authentication
        /// </summary>
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Authentication provider type
        /// </summary>
        [Required(ErrorMessage = "Auth provider is required")]
        [JsonPropertyName("authProvider")]
        public AuthProvider AuthProvider { get; set; }
        /// <summary>
        /// Biometric authentication type
        /// </summary>
        [JsonPropertyName("biometricType")]
        public BiometricType BiometricType { get; set; } = BiometricType.None;

        /// <summary>
        /// Biometric data (encrypted)
        /// </summary>
        [JsonPropertyName("biometricData")]
        public string? BiometricData { get; set; }
        /// <summary>
        /// Device identifier for biometric authentication
        /// </summary>
        [JsonPropertyName("deviceId")]
        public string? DeviceId { get; set; }
        /// <summary>
        /// Last known successful authentication method for this device
        /// </summary>
        [JsonPropertyName("lastAuthMethod")]
        public string? LastAuthMethod { get; set; }

    }
    /// <summary>
    /// Extension methods for LoginDto validation and processing
    /// </summary>
    public static class LoginDtoExtensions
    {
        /// <summary>
        /// Validates the login request based on authentication method
        /// </summary>
        public static bool IsValid(this LoginDto loginDto)
        {
            return loginDto.AuthProvider switch
            {
                AuthProvider.Email => !string.IsNullOrEmpty(loginDto.Email) &&
                                    (!string.IsNullOrEmpty(loginDto.Password) ||
                                     loginDto.BiometricType != BiometricType.None),

                AuthProvider.Phone => !string.IsNullOrEmpty(loginDto.Phone) &&
                                    (!string.IsNullOrEmpty(loginDto.Password) ||
                                     loginDto.BiometricType != BiometricType.None),

                AuthProvider.Google => true,

                _ => false
            };

        }
        /// <summary>
        /// Validates biometric authentication requirements
        /// </summary>
        public static bool IsValidBiometric(this LoginDto login)
        {
            if (login.BiometricType == BiometricType.None)
                return true;

            return !string.IsNullOrEmpty(login.BiometricData) &&
                   !string.IsNullOrEmpty(login.DeviceId);
        }

        /// <summary>
        /// Sanitizes the login data before processing
        /// </summary>
        public static LoginDto Sanitize(this LoginDto login)
        {
            login.Email = login.Email?.Trim().ToLower();
            login.Phone = login.Phone?.Trim();
            login.DeviceId = login.DeviceId?.Trim();
            return login;
        }
    }

}