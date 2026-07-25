using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform contentParent; // Drag 'Content' here
    [SerializeField] private UpgradeItemUI itemPrefab;  // Drag 'Button Prefab' here

    [Header("Shop Data")]
    [SerializeField] private List<UpgradeData> upgradesList;

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
}