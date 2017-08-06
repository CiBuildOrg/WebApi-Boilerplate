namespace App.Security.Password
{
    /// <summary>
    /// Password weak indicator
    /// </summary>
    public enum PasswordStrength
    {
        /// <summary>
        /// Undefined
        /// </summary>
        Undefined,
        /// <summary>
        /// Password is very weak = don't use
        /// </summary>
        VeryWeak,
        /// <summary>
        /// Password is weak = don't use
        /// </summary>
        Weak,
        /// <summary>
        /// Password is medium weak = warn
        /// </summary>
        Medium,
        /// <summary>
        /// Password is normal - use
        /// </summary>
        Normal,
        /// <summary>
        /// Password is strong - use
        /// </summary>
        Strong
    }
}