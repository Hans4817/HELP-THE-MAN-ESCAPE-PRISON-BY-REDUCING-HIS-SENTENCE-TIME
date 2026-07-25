using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Data", menuName = "Shop/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Upgrade Info")]
    public string upgradeName = "New Upgrade";
    public int price = 100;
    public Sprite icon;
}