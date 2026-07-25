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

    public void Setup(UpgradeData data)
    {
        if (titleText != null) titleText.text = data.upgradeName;
        if (priceText != null) priceText.text = $"${data.price}";
        if (iconImage != null && data.icon != null) iconImage.sprite = data.icon;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() =>
        {
            Debug.Log($"Purchased: {data.upgradeName} for ${data.price}!");
        });
    }
}