using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemUI : MonoBehaviour
{
    [Header("UI Component References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button buyButton;

    // Price Tracking
    private double basePrice;
    private double currentPrice;
    private int currentLevel = 0;
    
    // Exponential Growth Factor (1.15 = 15% increase per level)
    [SerializeField] private double priceGrowthFactor = 1.15;

    public void Setup(UpgradeData data, SpawnArea spawnArea)
    {
        if (data == null) return;

        // Ensure price growth factor is never <= 1 to avoid broken calculations
        if (priceGrowthFactor <= 1.0) priceGrowthFactor = 1.15;

        basePrice = data.price;
        CalculateCurrentPrice();

        if (titleText != null) titleText.text = data.upgradeName;
        UpdatePriceUI();
        if (iconImage != null && data.icon != null) iconImage.sprite = data.icon;

        if (buyButton == null)
        {
            Debug.LogError($"Buy button reference is missing on {gameObject.name}!", this);
            return;
        }

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            if (CurrencyManager.Instance != null && CurrencyManager.Instance.CanAffordMoney(currentPrice))
            {
                // Optional: Deduct currency if CanAffordMoney doesn't automatically do so
                // CurrencyManager.Instance.SubtractMoney(currentPrice);

                Debug.Log($"Purchased: {data.upgradeName} Level {currentLevel + 1} for ${currentPrice:N0}!");

                // 1. Apply effect based on UpgradeType from UpgradeData
                switch (data.upgradeType)
                {
                    case UpgradeType.IncomeMultiplier:
                        CurrencyManager.Instance.AddMoneyMultiplier(data.upgradeValue);
                        break;

                    case UpgradeType.Clicker:
                    case UpgradeType.AlarmClock:
                        CurrencyManager.Instance.SubtractSentenceTime(data.upgradeValue);
                        break;
                }

                // 2. Spawn 3D prefab at random position in area
                if (data.prefabToSpawn != null)
                {
                    Vector3 spawnPosition = spawnArea != null ? spawnArea.GetRandomSpawnPosition() : Vector3.zero;
                    Quaternion spawnRotation = spawnArea != null ? spawnArea.transform.rotation : Quaternion.identity;

                    Instantiate(data.prefabToSpawn, spawnPosition, spawnRotation);
                }

                // 3. Increment level, recalculate exponential price, and update UI
                currentLevel++;
                CalculateCurrentPrice();
                UpdatePriceUI();
            }
            else
            {
                Debug.Log($"Not enough money to buy {data.upgradeName}!");
            }
        });
    }

    private void CalculateCurrentPrice()
    {
        // Exponential formula: BasePrice * (GrowthFactor ^ Level)
        currentPrice = Math.Round(basePrice * Math.Pow(priceGrowthFactor, currentLevel));
    }

    private void UpdatePriceUI()
    {
        if (priceText != null) priceText.text = $"${currentPrice:N0}";
    }
}