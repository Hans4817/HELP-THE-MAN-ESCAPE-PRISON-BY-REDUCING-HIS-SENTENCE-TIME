using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    // Currency Events
    public static event Action<double> OnMoneyChanged;
    public static event Action<double> OnSentenceTimeChanged;

    // Multiplier Events (useful if you want to display "2x" on your UI)
    public static event Action<double> OnMoneyMultiplierChanged;

    // Currencies with default starting values
    public double Money { get; private set; } = 10;
    public double SentenceTime { get; private set; } = 1e18d;

    // Multipliers (Default is 1.0x -> normal earnings)
    public double MoneyMultiplier { get; private set; } = 1.0;
    public double SentenceTimeMultiplier { get; private set; } = 1.0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #region Money & Multipliers
    public void AddMoney(double baseAmount)
    {
        if (baseAmount <= 0) return;

        // Multiply base income by current multiplier
        double finalAmount = baseAmount * MoneyMultiplier;

        Money += finalAmount;
        OnMoneyChanged?.Invoke(Money);

        SubtractSentenceTime(finalAmount);
    }

    public void SubtractMoney(double baseAmount)
    {
        if (baseAmount <= 0) return;

        Money -= baseAmount;
        OnMoneyChanged?.Invoke(Money);
    }

    public bool CanAffordMoney(double amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            OnMoneyChanged?.Invoke(Money);
            return true;
        }
        return false;
    }

    /// Adds to the current multiplier (e.g., passing 0.5 increases 1.0x to 1.5x).
    public void AddMoneyMultiplier(double amount)
    {
        if (amount <= 0) return;

        MoneyMultiplier += amount;
        OnMoneyMultiplierChanged?.Invoke(MoneyMultiplier);
        Debug.Log($"Money Multiplier increased! New Multiplier: {MoneyMultiplier}x");
    }
    #endregion

    #region SentenceTime
    public void SubtractSentenceTime(double baseAmount)
    {
        if (baseAmount <= 0) return;

        double finalAmount = baseAmount * SentenceTimeMultiplier;

        SentenceTime -= finalAmount;
        if (SentenceTime < 0) Debug.LogWarning("WINNING");
        OnSentenceTimeChanged?.Invoke(SentenceTime);
    }

    // Example: Slash sentence time by 15% (multiplies current time by 0.85)
    public void MultiplySentenceTime(double factor)
    {
        SentenceTime *= factor; // e.g., factor = 0.85 for a 15% drop
        
        if (SentenceTime <= 0)
        {
            SentenceTime = 0;
            Debug.LogWarning("WINNING");
        }

        OnSentenceTimeChanged?.Invoke(SentenceTime);
    }

    public void AddSentenceTimeMultiplier(double amount)
    {
        if (amount <= 0) return;

        SentenceTimeMultiplier += amount;
    }
    #endregion
}