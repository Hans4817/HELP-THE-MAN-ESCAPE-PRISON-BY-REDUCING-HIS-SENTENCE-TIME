using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private BoxCollider spawnZone;

    private void Reset()
    {
        // Automatically grabs or adds a BoxCollider when attached
        spawnZone = GetComponent<BoxCollider>();
        if (spawnZone != null) spawnZone.isTrigger = true;
    }

    /// <summary>
    /// Calculates a random position inside the BoxCollider boundaries.
    /// </summary>
    public Vector3 GetRandomSpawnPosition()
    {
        if (spawnZone == null)
        {
            Debug.LogWarning("SpawnArea: Missing BoxCollider reference! Using transform position.");
            return transform.position;
        }

        Bounds bounds = spawnZone.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, randomY, randomZ);
    }
}