// made the shop system using gemini
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    // Singleton instance so buttons can access ShopManager easily
    public static ShopManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform contentParent; // Drag 'Content' here
    [SerializeField] private UpgradeItemUI itemPrefab;  // Drag 'Button Prefab' here

    [Header("Shop Data")]
    [SerializeField] private List<UpgradeData> upgradesList;
    
    private void Awake()
    {
        // Set up Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GenerateShop();
    }

    // Right-click the ShopManager header in the Inspector and click "Preview Shop in Editor"
    [ContextMenu("Preview Shop in Editor")]
    public void GenerateShopInEditor()
    {
        GenerateShop();
    }

    public void GenerateShop()
    {
        if (contentParent == null || itemPrefab == null)
        {
            Debug.LogWarning("ShopManager: Missing references! Please assign Content Parent and Item Prefab.");
            return;
        }

        // 1. Clear existing UI children safely
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                Destroy(contentParent.GetChild(i).gameObject);
            else
                DestroyImmediate(contentParent.GetChild(i).gameObject);
        }

        // 2. Instantiate new shop items from ScriptableObjects
        if (upgradesList == null) return;

        foreach (UpgradeData data in upgradesList)
        {
            if (data == null) continue;

            UpgradeItemUI newItem = Instantiate(itemPrefab, contentParent);
            newItem.Setup(data);
        }
    }

    // Call this method when a buy button is pressed!
    public void BuyUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null) return;

        // 1. Check if the player can afford it
        if (CurrencyManager.Instance.CanAffordMoney(upgrade.price))
        {
            Debug.LogWarning($"Cannot afford {upgrade.upgradeName}! Need {upgrade.price} coins, but only have {CurrencyManager.Instance.Money}.");
            return;
        }

        // 2. Deduct currency
        CurrencyManager.Instance.SubtractMoney(2);
        Debug.Log($"Bought {upgrade.upgradeName} for {upgrade.price} coins! Remaining coins: {CurrencyManager.Instance.Money}");

        // 3. Apply effect based on UpgradeType
        switch (upgrade.upgradeType)
        {
            case UpgradeType.Counter:
                Debug.Log($"[Effect Applied] Money Status: {CurrencyManager.Instance.Money}");
                break;

            case UpgradeType.IncomeMultiplier:
                Debug.Log($"[Effect Applied] Increased Income Multiplier by {upgrade.upgradeValue}!");
                break;

            default:
                Debug.LogWarning($"Unhandled UpgradeType: {upgrade.upgradeType}");
                break;
        }
    }
}