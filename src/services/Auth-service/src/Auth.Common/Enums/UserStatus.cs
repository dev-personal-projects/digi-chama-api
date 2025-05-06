/// <summary>
/// Represents the various lifecycle states of a user account in the system.
/// 
/// <para>
/// This enum is essential for managing authentication and authorization flows 
/// by allowing services to determine what actions are permitted based on the user's current status.
/// </para>
/// 
/// <para>
/// These statuses are typically used in login workflows, access control, user notifications,
/// and audit logging. Each status should directly influence how the system responds to user actions.
/// </para>
/// 
/// <para><b>Example Scenario:</b></para>
/// <para>
/// In a fintech platform, a user account may be created but require email or phone verification. 
/// Until verified, the account would remain in the <see cref="UserStatus.Unverified"/> state and 
/// would not be permitted to perform financial transactions. If multiple failed login attempts occur,
/// the account could be moved to the <see cref="UserStatus.Locked"/> state to prevent abuse.
/// </para>
/// </summary>
namespace Auth.Common.Enums
{

    public enum UserStatus
    {
        /// <summary>
        /// Account has been created but not yet verified via email, phone, or other method.
        /// Typically restricted from accessing secure resources until verification is complete.
        /// </summary>
        Unverified = 1,

        /// <summary>
        /// Account has successfully passed the verification process and is fully active.
        /// </summary>
        Verified = 2,

        /// <summary>
        /// Account has been temporarily disabled, possibly due to suspicious activity, 
        /// policy violations, or failed login attempts.
        /// </summary>
        Locked = 3,

        /// <summary>
        /// Account has been voluntarily deactivated by the user. 
        /// May be reactivated based on system rules or user request.
        /// </summary>
        Deactivated = 4,
        /// <summary>
        /// Account has been suspended by administration
        /// </summary>
        Suspended = 5,
        ///<summary>
        /// account has been deleted by the user or admin
        /// </summary>
        Deleted = 6
    }
}
