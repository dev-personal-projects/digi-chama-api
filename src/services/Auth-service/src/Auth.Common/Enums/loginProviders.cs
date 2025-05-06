namespace Auth.Common.Enums
{
    /// <summary>
    /// Defines the types of biometric authentication supported
    /// </summary>
    public enum BiometricType
    {
        /// <summary>
        /// No biometric authentication
        /// </summary>
        None = 0,

        /// <summary>
        /// Fingerprint authentication
        /// </summary>
        Fingerprint = 1,

        /// <summary>
        /// Face recognition authentication
        /// </summary>
        FaceId = 2,

        /// <summary>
        /// Iris scanning authentication
        /// </summary>
        IrisScan = 3
    }
}