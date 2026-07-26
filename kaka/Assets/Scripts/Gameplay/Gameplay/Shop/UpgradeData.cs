using UnityEngine;

// Creates an entry in the Unity Create menu so you can make Upgrade files in your Project window
[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Shop/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("UI Display")]
    public string upgradeName;  // Name shown on the button label
    public int price;           // Cost of the upgrade
    public Sprite icon;         // Icon graphic shown on the button

    [Header("Upgrade Settings")]
    public UpgradeType upgradeType; // Which stat to boost (Speed, JumpPower, etc.)
    public float upgradeValue;      // The numerical boost amount (e.g., +2.5 speed)
}

// Defines all available upgrade types in your game
public enum UpgradeType 
{ 
    Counter, 
    IncomeMultiplier 
}