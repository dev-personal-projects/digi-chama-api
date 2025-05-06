
namespace Auth.Common.Enums
{
    /// <summary>
    /// Defines the initial role of a user in the system.
    /// Group-specific roles are handled separately.
    /// </summary>
    public enum UserRole
    {

        /// <summary>
        /// User has not been allocated any specific role yet
        /// </summary>
        Notallocated = 1,
        /// <summary>
        /// User is an administrator of at least one group
        /// </summary>
        Admin = 2,
        /// <summary>
        /// User is a member of at least one group
        /// </summary>
        Member = 3,

        ///<summary
        /// i developer and ceo i am the super admin of the application
        /// </summmary>
        superAdmin = 4


    }

}