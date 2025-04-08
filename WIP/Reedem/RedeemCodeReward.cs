using UnityEngine;

/// <summary>
/// Abstract ScriptableObject base class for defining rewards that can be granted by redemption codes.
/// Concrete implementations should extend this class to define specific reward behaviors.
/// </summary>
[CreateAssetMenu(fileName = "New Reward", menuName = "Reward System/Abstract Reward")]
public abstract class RedeemCodeReward : ScriptableObject
{
    [Header("Reward Information")]
    [Tooltip("Unique identifier for this reward")]
    public string rewardId;
    
    [Tooltip("Display name of the reward")]
    public string displayName;
    
    [TextArea(3, 5)]
    [Tooltip("Description of what this reward provides")]
    public string description;
    
    [Tooltip("Icon representing this reward")]
    public Sprite icon;

    [Tooltip("Rarity or tier of this reward")]
    public RewardRarity rarity = RewardRarity.Common;
    
    [Tooltip("Category this reward belongs to")]
    public RewardCategory category = RewardCategory.Miscellaneous;

    /// <summary>
    /// Abstract method that must be implemented by derived classes to grant the reward to a player.
    /// </summary>
    /// <param name="playerId">The ID of the player to grant the reward to</param>
    /// <param name="amount">The amount of the reward to grant</param>
    /// <param name="additionalData">Optional JSON string containing additional data for granting the reward</param>
    /// <returns>True if the reward was successfully granted, false otherwise</returns>
    public abstract bool GrantReward(string playerId, int amount = 1, string additionalData = null);

    /// <summary>
    /// Virtual method that can be overridden to check if a player is eligible to receive this reward.
    /// </summary>
    /// <param name="playerId">The ID of the player to check</param>
    /// <returns>True if the player is eligible, false otherwise</returns>
    public virtual bool CanReceiveReward(string playerId)
    {
        // Default implementation assumes all players are eligible
        return true;
    }

    /// <summary>
    /// Virtual method that can be overridden to provide a formatted display string for the reward.
    /// </summary>
    /// <param name="amount">The amount of the reward</param>
    /// <returns>A formatted string describing the reward</returns>
    public virtual string GetDisplayString(int amount = 1)
    {
        return amount > 1 ? $"{amount}x {displayName}" : displayName;
    }

    /// <summary>
    /// Enumeration defining different rarity levels for rewards.
    /// </summary>
    public enum RewardRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic,
        Limited,
        Event
    }

    /// <summary>
    /// Enumeration defining different categories of rewards.
    /// </summary>
    public enum RewardCategory
    {
        Currency,
        Item,
        Character,
        Skin,
        PowerUp,
        VIP,
        LevelUnlock,
        Consumable,
        Booster,
        Cosmetic,
        Achievement,
        Miscellaneous
    }
}
