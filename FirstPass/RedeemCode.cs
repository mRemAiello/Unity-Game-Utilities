using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RedeemCode
{
    public string code = "GENSHINCODE";
    public string description = "A redeem code with rewards";
    
    // Basic rewards (like Genshin Impact)
    public int primogems = 0;
    public int mora = 0;
    
    // Expiration settings
    public bool hasExpiration = true;
    public DateTime expirationDate = DateTime.Now.AddDays(30);
    
    // Additional items
    public List<RewardItem> items = new List<RewardItem>();
}

[System.Serializable]
public class RewardItem
{
    public enum ItemType
    {
        Currency,
        Material,
        Weapon,
        Character,
        Artifact
    }
    
    public ItemType type;
    public string itemId;
    public string itemName;
    public int amount = 1;
    public Sprite icon;
}
