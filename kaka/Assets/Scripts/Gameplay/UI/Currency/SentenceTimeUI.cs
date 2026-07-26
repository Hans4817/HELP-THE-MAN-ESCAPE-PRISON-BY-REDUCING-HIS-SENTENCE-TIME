// SentenceTimeUI.cs attached to the SentenceTime text object
using UnityEngine;
using TMPro;

public class SentenceTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI SentenceTimeText;

    private void OnEnable()
    {
        CurrencyManager.OnSentenceTimeChanged += UpdateUI;
    }

    private void OnDisable()
    {
        CurrencyManager.OnSentenceTimeChanged -= UpdateUI;
    }

    private void Start()
    {
        if (CurrencyManager.Instance != null) UpdateUI(CurrencyManager.Instance.SentenceTime);
    }

    private void UpdateUI(double amount)
    {
        SentenceTimeText.text = $"{NumberFormatter.Format(amount)}";
    }
}