using UnityEngine;

/// <summary>
/// Enumeration representing the possible statuses of a redemption code in a reward system.
/// </summary>
public enum ReededCodeStatus
{
    /// <summary>
    /// The code is valid and has not been redeemed yet.
    /// </summary>
    Valid,
    
    /// <summary>
    /// The code has been successfully redeemed.
    /// </summary>
    Redeemed,
    
    /// <summary>
    /// The code is invalid (does not exist in the system).
    /// </summary>
    Invalid,
    
    /// <summary>
    /// The code has expired and can no longer be redeemed.
    /// </summary>
    Expired,
    
    /// <summary>
    /// The code has already been redeemed.
    /// </summary>
    AlreadyRedeemed,
    
    /// <summary>
    /// The code is valid but the user does not meet the requirements to redeem it.
    /// </summary>
    RequirementsNotMet,
    
    /// <summary>
    /// An error occurred during the redemption process.
    /// </summary>
    Error,
    
    /// <summary>
    /// The system is currently processing the code.
    /// </summary>
    Processing
}
