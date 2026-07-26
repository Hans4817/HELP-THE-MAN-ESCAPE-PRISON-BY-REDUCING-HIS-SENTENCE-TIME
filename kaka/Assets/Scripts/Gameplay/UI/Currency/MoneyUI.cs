// MoneyUI.cs attached to the Money text object
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyText;

    private void OnEnable()
    {
        CurrencyManager.OnMoneyChanged += UpdateUI;
    }

    private void OnDisable()
    {
        CurrencyManager.OnMoneyChanged -= UpdateUI;
    }

    private void Start()
    {
        if (CurrencyManager.Instance != null) UpdateUI(CurrencyManager.Instance.Money);
    }

    private void UpdateUI(double amount)
    {
        MoneyText.text = $"{amount:N0}";
    }
}