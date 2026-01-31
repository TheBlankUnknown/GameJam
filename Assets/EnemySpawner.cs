using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject enemyType1;
    public GameObject enemyType2;
    public GameObject enemyType3;

    [Header("Spawn Settings")]
    public Transform spawnPoint;       // Where enemies appear
    public float spawnInterval = 5f;   // Seconds between spawns
    public bool randomizePosition = false; // Spawn in random area
    public Vector3 spawnAreaSize = new Vector3(5f, 0f, 5f); // Only used if randomizePosition

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // Pick a random enemy type
        int randomIndex = Random.Range(0, 3);
        GameObject enemyPrefab = null;

        switch (randomIndex)
        {
            case 0:
                enemyPrefab = enemyType1;
                break;
            case 1:
                enemyPrefab = enemyType2;
                break;
            case 2:
                enemyPrefab = enemyType3;
                break;
        }

        if (enemyPrefab == null) return;

        // Determine spawn position
        Vector3 spawnPos = spawnPoint.position;
        if (randomizePosition)
        {
            spawnPos += new Vector3(
                Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
                Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f),
                Random.Range(-spawnAreaSize.z / 2f, spawnAreaSize.z / 2f)
            );
        }

        // Spawn enemy
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
