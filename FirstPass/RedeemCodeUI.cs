using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RedeemCodeUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField codeInputField;
    [SerializeField] private Button redeemButton;
    [SerializeField] private GameObject redeemPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private TMP_Text errorMessage;
    [SerializeField] private TMP_Text successMessage;
    
    [Header("Reward Display")]
    [SerializeField] private Transform rewardItemsContainer;
    [SerializeField] private GameObject rewardItemPrefab;
    
    [Header("Animation")]
    [SerializeField] private float panelAnimDuration = 0.3f;
    
    // References
    private Animator panelAnimator;
    
    private void Start()
    {
        // Get references
        panelAnimator = redeemPanel.GetComponent<Animator>();
        
        // Add button listeners
        redeemButton.onClick.AddListener(OnRedeemButtonClicked);
        closeButton.onClick.AddListener(ClosePanel);
        
        // Start with panels hidden
        successPanel.SetActive(false);
        errorPanel.SetActive(false);
        
        // Optional: Start with panel hidden if you want to show it with a button
        // redeemPanel.SetActive(false);
    }
    
    public void ShowPanel()
    {
        redeemPanel.SetActive(true);
        codeInputField.text = "";
        
        // Animate panel if you have an animator
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger("Show");
        }
    }
    
    public void ClosePanel()
    {
        // Animate panel if you have an animator
        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger("Hide");
            StartCoroutine(DisablePanelAfterDelay(panelAnimDuration));
        }
        else
        {
            redeemPanel.SetActive(false);
        }
    }
    
    private IEnumerator DisablePanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        redeemPanel.SetActive(false);
    }
    
    private void OnRedeemButtonClicked()
    {
        string code = codeInputField.text.Trim();
        
        if (string.IsNullOrEmpty(code))
        {
            ShowError("Please enter a valid redeem code.");
            return;
        }
        
        // Try to redeem the code
        bool success = RedeemCodeManager.Instance.RedeemCode(code);
        
        if (success)
        {
            ShowSuccess("Code redeemed successfully!");
            // In a real implementation, you would display the rewards here
            // DisplayRewards(rewards);
        }
        else
        {
            ShowError("Invalid code or already redeemed.");
        }
    }
    
    private void ShowSuccess(string message)
    {
        successPanel.SetActive(true);
        errorPanel.SetActive(false);
        successMessage.text = message;
        
        // Auto-hide after a few seconds
        StartCoroutine(HideMessageAfterDelay(successPanel, 3f));
    }
    
    private void ShowError(string message)
    {
        errorPanel.SetActive(true);
        successPanel.SetActive(false);
        errorMessage.text = message;
        
        // Auto-hide after a few seconds
        StartCoroutine(HideMessageAfterDelay(errorPanel, 3f));
    }
    
    private IEnumerator HideMessageAfterDelay(GameObject panel, float delay)
    {
        yield return new WaitForSeconds(delay);
        panel.SetActive(false);
    }
    
    // Method to display the rewards from a redeemed code
    private void DisplayRewards(RedeemCode code)
    {
        // Clear any existing reward items
        foreach (Transform child in rewardItemsContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Create a reward display for primogems if any
        if (code.primogems > 0)
        {
            AddRewardDisplay("Primogems", code.primogems.ToString());
        }
        
        // Create a reward display for mora if any
        if (code.mora > 0)
        {
            AddRewardDisplay("Mora", code.mora.ToString());
        }
        
        // Add other items
        foreach (var item in code.items)
        {
            AddRewardDisplay(item.itemName, item.amount.ToString());
        }
    }
    
    private void AddRewardDisplay(string itemName, string amount)
    {
        // Instantiate the reward item prefab
        GameObject rewardItem = Instantiate(rewardItemPrefab, rewardItemsContainer);
        
        // Set the item name and amount
        Transform nameText = rewardItem.transform.Find("ItemName");
        Transform amountText = rewardItem.transform.Find("ItemAmount");
        Transform icon = rewardItem.transform.Find("ItemIcon");
        
        if (nameText != null)
        {
            nameText.GetComponent<TMP_Text>().text = itemName;
        }
        
        if (amountText != null)
        {
            amountText.GetComponent<TMP_Text>().text = "x" + amount;
        }
        
        // Set the icon if you have one
        // if (icon != null && itemIcon != null)
        // {
        //     icon.GetComponent<Image>().sprite = itemIcon;
        // }
    }
}
