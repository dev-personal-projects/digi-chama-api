using System;
using System.Text.Json.Serialization;
using Auth.Common.Enums;
namespace Auth.Core.DTOs
{
    /// <summary>
    /// Data transfer object for authentication response.
    /// Follows REST API best practices and includes comprehensive authentication details.
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// JWT access token for API authentication
        /// </summary>
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; } = string.Empty;
        /// <summary>
        /// Refresh token for obtaining new access tokens
        /// </summary>
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } = string.Empty;
        /// <summary>
        /// Token type (usually "Bearer")
        /// </summary>
        [JsonPropertyName("tokenType")]
        public string TokenType { get; set; } = "Bearer";
        /// <summary>
        /// Access token expiration time in seconds
        /// </summary>
        [JsonPropertyName("expiresIn")]
        public int ExpiresIn { get; set; }
        /// <summary>
        /// Basic user information included in the response
        /// </summary>
        [JsonPropertyName("user")]
        public UserDto User { get; set; } = null!;
        /// <summary>
        /// Timestamp of successful authentication
        /// </summary>
        [JsonPropertyName("authenticatedAt")]
        public DateTime AuthenticatedAt { get; set; } = DateTime.UtcNow;


    }
    /// <summary>
    /// Nested DTO containing essential user information
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Unique identifier of the user
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Full name of the user
        /// </summary>
        [JsonPropertyName("fullName")]
        public string FullName { get; set; } = string.Empty;
        /// <summary>
        /// Email address (if available)
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        /// <summary>
        /// Phone number (if available)
        /// </summary>
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        /// <summary>
        /// Authentication provider used
        /// </summary>
        [JsonPropertyName("authProvider")]
        public AuthProvider AuthProvider { get; set; }
        /// <summary>
        /// User's current role
        /// </summary>
        [JsonPropertyName("role")]
        public UserRole Role { get; set; }
        /// <summary>
        /// Account verification status
        /// </summary>
        [JsonPropertyName("status")]
        public UserStatus Status { get; set; }
        /// <summary>
        /// URL to user's profile picture (if available)
        /// </summary>
        [JsonPropertyName("profilePictureUrl")]
        public string? ProfilePictureUrl { get; set; }

    }
    /// <summary>
    /// Extension methods for AuthResponseDto
    /// </summary>
    public static class AuthResponseDtoExtensions
    {
        /// <summary>
        /// Creates a successful authentication response
        /// </summary>
        public static AuthResponseDto createSucess(
            string accessToken,
            string refreshToken,
            int expiresIn,
            UserDto user

        )
        {
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = expiresIn,
                User = user
            };
        }
        /// <summary>
        /// Validates if the authentication response is complete and valid
        /// </summary>
        public static bool IsValid(this AuthResponseDto response)
        {
            return !string.IsNullOrEmpty(response.AccessToken) &&
                           !string.IsNullOrEmpty(response.RefreshToken) &&
                           response.ExpiresIn > 0 &&
                           response.User != null &&
                           !string.IsNullOrEmpty(response.User.Id);
        }

    }

}