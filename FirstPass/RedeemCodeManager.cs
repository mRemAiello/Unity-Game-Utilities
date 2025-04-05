using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedeemCodeManager : MonoBehaviour
{
    // Singleton pattern
    public static RedeemCodeManager Instance { get; private set; }
    
    // List of valid redeem codes
    [SerializeField] private List<RedeemCode> validCodes = new List<RedeemCode>();
    
    // List of already redeemed codes
    private List<string> redeemedCodes = new List<string>();

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public bool RedeemCode(string code)
    {
        // Convert the code to uppercase for case-insensitive comparison
        code = code.ToUpper().Trim();
        
        // Check if the code has already been redeemed
        if (redeemedCodes.Contains(code))
        {
            Debug.Log("This code has already been redeemed: " + code);
            return false;
        }
        
        // Find the code in our valid codes list
        RedeemCode validCode = validCodes.Find(c => c.code == code);
        
        // If code isn't valid
        if (validCode == null)
        {
            Debug.Log("Invalid redeem code: " + code);
            return false;
        }
        
        // Check if the code has expired
        if (validCode.hasExpiration && System.DateTime.Now > validCode.expirationDate)
        {
            Debug.Log("This code has expired: " + code);
            return false;
        }
        
        // Add rewards to player's inventory
        GiveRewards(validCode);
        
        // Mark code as redeemed
        redeemedCodes.Add(code);
        
        Debug.Log("Code redeemed successfully: " + code);
        return true;
    }
    
    private void GiveRewards(RedeemCode code)
    {
        // In a real game, this would add items to player inventory
        Debug.Log($"Rewarded primogems: {code.primogems}");
        Debug.Log($"Rewarded mora: {code.mora}");
        
        // Process other rewards
        foreach (var item in code.items)
        {
            Debug.Log($"Rewarded item: {item.amount}x {item.itemName}");
        }
    }
    
    // Method to add new codes (for server updates or admin tools)
    public void AddRedeemCode(RedeemCode newCode)
    {
        // Make sure the code is in uppercase for consistency
        newCode.code = newCode.code.ToUpper().Trim();
        
        // Check if the code already exists
        if (validCodes.Exists(c => c.code == newCode.code))
        {
            Debug.Log("A code with this ID already exists: " + newCode.code);
            return;
        }
        
        validCodes.Add(newCode);
        Debug.Log("Added new redeem code: " + newCode.code);
    }
}
