using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    // Events for each currency so UI elements update independently
    public static event Action<double> OnMoneyChanged;
    public static event Action<double> OnSentenceTimeChanged;

    public double money { get; private set; }
    public double sentenceTime { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #region Money Logic
    public void AddMoney(double amount)
    {
        if (amount <= 0) return;
        money += amount;
        OnMoneyChanged?.Invoke(money);
    }

    public void SubtractMoney(double amount)
    {
        if (amount >= 0) return;
        money -= amount;
        OnMoneyChanged?.Invoke(money);
    }

    public bool TrySpendMoney(double amount)
    {
        if (money >= amount)
        {
            money -= amount;
            OnMoneyChanged?.Invoke(money);
            return true;
        }
        return false;
    }
    #endregion

    #region SentenceTime Logic
    public void AddSentenceTime(double amount)
    {
        if (amount <= 0) return;
        sentenceTime += amount;
        OnSentenceTimeChanged?.Invoke(sentenceTime);
    }

    public bool TrySpendSentenceTime(double amount)
    {
        if (sentenceTime >= amount)
        {
            sentenceTime -= amount;
            OnSentenceTimeChanged?.Invoke(sentenceTime);
            return true;
        }
        return false;
    }
    #endregion
}