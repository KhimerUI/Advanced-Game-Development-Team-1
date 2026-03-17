using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private bool autoSpawn = true;

    [Header("Spawn Area")]
    [SerializeField] private bool useRandomOffset = false;
    [SerializeField] private float randomOffsetRange = 2f;

    private float spawnTimer = 0f;
    private int currentEnemyCount = 0;

    void Update()
    {
        if (!autoSpawn) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f && currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned to the spawner!");
            return;
        }

        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position;

        // Add random offset if enabled
        if (useRandomOffset)
        {
            spawnPos += Random.insideUnitSphere * randomOffsetRange;
            spawnPos.y = spawnPoint != null ? spawnPoint.position.y : transform.position.y;
        }

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemyCount++;
    }

    public void StopSpawning()
    {
        autoSpawn = false;
    }

    public void StartSpawning()
    {
        autoSpawn = true;
    }

    public void SetSpawnInterval(float interval)
    {
        spawnInterval = Mathf.Max(0.1f, interval);
    }

    public void SetMaxEnemies(int max)
    {
        maxEnemies = Mathf.Max(1, max);
    }

    public int GetCurrentEnemyCount()
    {
        return currentEnemyCount;
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}
