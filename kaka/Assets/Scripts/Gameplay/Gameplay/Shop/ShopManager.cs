using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private UpgradeItemUI itemPrefab;

    [Header("Randomized Spawn Area")]
    [SerializeField] private SpawnArea customSpawnArea; // Drag your SpawnArea GameObject here!

    [Header("Shop Data")]
    [SerializeField] private List<UpgradeData> upgradesList;

    private void Start()
    {
        GenerateShop();
    }

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

        // Clear existing UI children safely
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
                Destroy(contentParent.GetChild(i).gameObject);
            else
                DestroyImmediate(contentParent.GetChild(i).gameObject);
        }

        if (upgradesList == null) return;

        // Instantiate shop buttons and pass data + spawn area
        foreach (UpgradeData data in upgradesList)
        {
            if (data == null) continue;

            UpgradeItemUI newItem = Instantiate(itemPrefab, contentParent);
            newItem.Setup(data, customSpawnArea); // Correctly passes SpawnArea
        }
    }
}