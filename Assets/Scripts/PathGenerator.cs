using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public float platformWidth = 5f;
    public float platformHeight = 0.5f;
    public float platformSpacing = 2f;
    public int initialPlatformCount = 10;
    public GameObject obstaclePrefab;
    public float obstacleYOffset = 0.5f;
    public float obstacleSpawnChance = 0.5f;

    private float nextPlatformY = 0f;

    void Start()
    {
        GenerateInitialPath();
    }

    void Update()
    {
        // Generate new platforms as the player ascends
        if (Camera.main.transform.position.y + 10f > nextPlatformY)
        {
            GeneratePlatform();
        }
    }

    void GenerateInitialPath()
    {
        for (int i = 0; i < initialPlatformCount; i++)
        {
            GeneratePlatform();
        }
    }

    void GeneratePlatform()
    {
        float x = Random.Range(-platformWidth / 2f, platformWidth / 2f);
        Vector3 platformPos = new Vector3(x, nextPlatformY, 0f);
        Instantiate(platformPrefab, platformPos, Quaternion.identity);

        // Randomly decide to spawn an obstacle on top of the platform
        if (obstaclePrefab != null && Random.value < obstacleSpawnChance)
        {
            Vector3 obstaclePos = new Vector3(x, nextPlatformY + obstacleYOffset, 0f);
            Instantiate(obstaclePrefab, obstaclePos, Quaternion.identity);
        }

        nextPlatformY += platformSpacing;
    }
} 