//Registration DTO for user registration requests
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Auth.Common.Enums;


namespace Auth.Core.DTOs
{
    /// <summary>
    /// Data transfer object for user registration requests.
    /// Implements validation and documentation best practices.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// Full name of the user
        /// </summary>
        /// 
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty; /// <summary>
                                                             /// Email address (required if phone is not provided)
                                                             /// </summary>
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        /// <summary>
        /// Phone number (required if email is not provided)
        /// </summary>
        [Phone(ErrorMessage = "Invalid phone number format")]
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        /// <summary>
        /// Password (required for email/phone authentication)
        /// </summary>
        [JsonPropertyName("password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character")]
        public string? Password { get; set; }
        /// <summary>
        /// Authentication provider type
        /// </summary>
        [Required(ErrorMessage = "Auth provider is required")]
        [JsonPropertyName("authProvider")]
        public AuthProvider AuthProvider { get; set; }
        /// <summary>
        /// OAuth ID for social login
        /// </summary>
        [JsonPropertyName("oauthId")]
        public string? OAuthId { get; set; }
        /// <summary>
        /// User's location information
        /// </summary>
        [Required(ErrorMessage = "Location is required")]
        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// URL to user's profile picture (optional)
        /// </summary>
        [Url(ErrorMessage = "Invalid URL format")]
        [JsonPropertyName("profilePictureUrl")]
        public string? ProfilePictureUrl { get; set; }

        /// <summary>
        /// Preferred language for communications (defaults to English)
        /// </summary>
        [JsonPropertyName("preferredLanguage")]
        public string PreferredLanguage { get; set; } = "en";
        /// <summary>
        /// Version of terms of service accepted by user
        /// </summary>
        [Required(ErrorMessage = "Terms acceptance is required")]
        [JsonPropertyName("acceptedTermsVersion")]
        public string AcceptedTermsVersion { get; set; } = string.Empty;


    }
    /// <summary>
    /// Extension methods for RegisterDto validation and processing
    /// </summary>
    public static class RegisterDtoExtensions
    {
        /// <summary>
        /// Validates the registration data based on auth provider
        /// </summary>



        public static bool Validate(this RegisterDto registerDto)
        {
            switch (registerDto.AuthProvider)
            {
                case AuthProvider.Email:
                    return !string.IsNullOrEmpty(registerDto.Email) &&
                           !string.IsNullOrEmpty(registerDto.Password);

                case AuthProvider.Phone:
                    return !string.IsNullOrEmpty(registerDto.Phone) &&
                           !string.IsNullOrEmpty(registerDto.Password);

                case AuthProvider.Google:
                    return !string.IsNullOrEmpty(registerDto.OAuthId);

                default:
                    return false;
            }
        }
        /// <summary>
        /// Sanitizes the registration data before processing
        /// </summary>
        public static RegisterDto Sanitize(this RegisterDto register)
        {
            register.Email = register.Email?.Trim().ToLower();
            register.Phone = register.Phone?.Trim();
            register.FullName = register.FullName.Trim();
            register.Location = register.Location.Trim();
            return register;
        }
    }
}