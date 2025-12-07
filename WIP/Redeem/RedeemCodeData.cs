using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject that contains data for a redemption code in a reward system.
/// </summary>
[CreateAssetMenu(fileName = "New Redeem Code", menuName = "Reward System/Redeem Code Data")]
public class RedeemCodeData : ScriptableObject
{
    [Header("Code Information")]
    [Tooltip("The unique code that users will enter to redeem rewards")]
    public string codeValue;
    
    [Tooltip("Display name for this code")]
    public string codeName;
    
    [TextArea(3, 5)]
    [Tooltip("Description of what this code provides")]
    public string description;
    
    [Tooltip("When this code will expire (leave blank for no expiration)")]
    public DateTime expirationDate;
    
    [Tooltip("Whether this code has an expiration date")]
    public bool hasExpiration = false;
    
    [Tooltip("Maximum number of times this code can be redeemed (0 for unlimited)")]
    public int maxRedemptions = 1;
    
    [Header("Rewards")]
    [Tooltip("List of rewards granted by this code")]
    public List<Reward> rewards = new List<Reward>();
    
    /// <summary>
    /// Checks if the code has expired.
    /// </summary>
    public bool IsExpired()
    {
        if (!hasExpiration)
            return false;
            
        return DateTime.Now > expirationDate;
    }
    
    /// <summary>
    /// Data structure that defines a reward given by the redemption code.
    /// </summary>
    [Serializable]
    public class Reward
    {
        [Tooltip("Name of the reward")]
        public string rewardName;
        
        [Tooltip("Type of reward (currency, item, skin, etc.)")]
        public RewardType rewardType;
        
        [Tooltip("ID of the item to be awarded (if applicable)")]
        public string itemId;
        
        [Tooltip("Amount of the reward to give")]
        public int amount = 1;
        
        [Tooltip("Additional data for the reward (JSON format)")]
        [TextArea(2, 4)]
        public string additionalData;
    }
    
    /// <summary>
    /// Types of rewards that can be granted by redemption codes.
    /// </summary>
    public enum RewardType
    {
        Currency,
        Item,
        Character,
        Skin,
        PowerUp,
        VIP,
        LevelUnlock,
        Custom
    }
}
