using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public float spawnInterval = 1.5f;
    public float minX = -2.5f, maxX = 2.5f;
    public float spawnYOffset = 8f;
    public float platformSpacing = 2f;
    public float platformYThreshold = 0.1f;

    private float nextSpawnY = 0f;
    private Transform player;

    public void SetTarget(Transform newTarget)
    {
        player = newTarget;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
            nextSpawnY = player.position.y + spawnYOffset;
    }

    void Update()
    {
        // Only spawn coins if the game has started
        if (GameManager.Instance != null && GameManager.Instance.isGameStarted && player != null)
        {
            if (player.position.y + spawnYOffset > nextSpawnY)
            {
                SpawnCoin();
                nextSpawnY += spawnInterval;
            }
        }
    }

    void SpawnCoin()
    {
        // Prevent coin from spawning at the same Y as a platform
        if (Mathf.Abs((nextSpawnY % platformSpacing)) < platformYThreshold)
        {
            // Skip this spawn
            return;
        }

        float x = Random.Range(minX, maxX);
        Vector3 spawnPos = new Vector3(x, nextSpawnY, 0f);
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }

    public void ResetSpawner()
    {
        if (player != null)
            nextSpawnY = player.position.y + spawnYOffset;
    }
} 