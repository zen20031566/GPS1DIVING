using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnParent;
    [SerializeField] private int maxEnemiesPerLayer = 10;
    [SerializeField] private float spawnCheckInterval = 5f;
    [SerializeField] private float spawnRadius = 30f; // How far from player to spawn
    [SerializeField] private float minSpawnDistance = 15f; // Min distance from player

    [Header("Spawn Boundaries")]
    [SerializeField] private Vector2 minSpawnBounds = new Vector2(-100f, -150f);
    [SerializeField] private Vector2 maxSpawnBounds = new Vector2(100f, 10f);

    [Header("Collision Check")]
    [SerializeField] private float spawnCheckRadius = 0.5f; // Radius to check for obstacles
    [SerializeField] private LayerMask obstacleLayer; // Layer for tiles/ground

    [Header("Enemy Data by Depth")]
    [SerializeField] private List<EnemyDataSO> surfaceEnemies = new List<EnemyDataSO>();
    [SerializeField] private List<EnemyDataSO> shallowEnemies = new List<EnemyDataSO>();
    [SerializeField] private List<EnemyDataSO> mediumEnemies = new List<EnemyDataSO>();
    [SerializeField] private List<EnemyDataSO> deepEnemies = new List<EnemyDataSO>();
    [SerializeField] private List<EnemyDataSO> abyssEnemies = new List<EnemyDataSO>();

    private Transform playerTransform;
    private float spawnTimer;
    private Dictionary<DepthLayer, List<Enemy>> activeEnemies = new Dictionary<DepthLayer, List<Enemy>>();

    private void Start()
    {
        playerTransform = GameManager.Instance.PlayerTransform;
        spawnTimer = spawnCheckInterval;

        // Initialize active enemies dictionary
        foreach (DepthLayer layer in System.Enum.GetValues(typeof(DepthLayer)))
        {
            activeEnemies[layer] = new List<Enemy>();
        }

        // Subscribe to enemy death event
        GameEventsManager.Instance.EnemyEvents.OnEnemyDied += HandleEnemyDied;
    }

    private void OnDestroy()
    {
        GameEventsManager.Instance.EnemyEvents.OnEnemyDied -= HandleEnemyDied;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            spawnTimer = spawnCheckInterval;
            TrySpawnEnemies();
        }
    }

    private void TrySpawnEnemies()
    {
        if (playerTransform == null) return;

        float playerDepth = playerTransform.position.y;
        DepthLayer currentLayer = GetDepthLayer(playerDepth);

        // Try to spawn enemies in current and adjacent layers
        SpawnForLayer(currentLayer);
        SpawnForAdjacentLayer(currentLayer, -1); // Layer below
        SpawnForAdjacentLayer(currentLayer, 1);  // Layer above
    }

    private void SpawnForLayer(DepthLayer layer)
    {
        // Clean up null references
        activeEnemies[layer].RemoveAll(e => e == null);

        // Check if we need more enemies for this layer
        if (activeEnemies[layer].Count >= maxEnemiesPerLayer)
            return;

        List<EnemyDataSO> enemyPool = GetEnemyPoolForLayer(layer);
        if (enemyPool == null || enemyPool.Count == 0)
            return;

        // Spawn enemies until we reach max or fail to find valid positions
        int attemptCount = 0;
        int maxAttempts = 5;

        while (activeEnemies[layer].Count < maxEnemiesPerLayer && attemptCount < maxAttempts)
        {
            attemptCount++;

            Vector2 spawnPosition = GetRandomSpawnPosition(layer);
            if (spawnPosition == Vector2.zero) continue; // Failed to find valid position

            EnemyDataSO enemyData = SelectRandomEnemy(enemyPool);
            if (enemyData == null) continue;

            SpawnEnemy(enemyData, spawnPosition, layer);
        }
    }

    private void SpawnForAdjacentLayer(DepthLayer currentLayer, int offset)
    {
        int layerIndex = (int)currentLayer + offset;
        if (layerIndex < 0 || layerIndex >= System.Enum.GetValues(typeof(DepthLayer)).Length)
            return;

        DepthLayer adjacentLayer = (DepthLayer)layerIndex;
        SpawnForLayer(adjacentLayer);
    }

    private Vector2 GetRandomSpawnPosition(DepthLayer layer)
    {
        if (playerTransform == null) return Vector2.zero;

        // Get depth range for this layer
        (float minDepth, float maxDepth) = GetDepthRange(layer);

        // Try to find a valid position
        for (int i = 0; i < 10; i++)
        {
            // Random angle around player
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(minSpawnDistance, spawnRadius);

            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
            Vector2 spawnPos = (Vector2)playerTransform.position + offset;

            // Clamp to depth layer
            spawnPos.y = Mathf.Clamp(spawnPos.y, minDepth, maxDepth);

            // Clamp to world bounds
            spawnPos.x = Mathf.Clamp(spawnPos.x, minSpawnBounds.x, maxSpawnBounds.x);
            spawnPos.y = Mathf.Clamp(spawnPos.y, minSpawnBounds.y, maxSpawnBounds.y);

            // Check if position is valid (not too close to player)
            if (Vector2.Distance(spawnPos, playerTransform.position) >= minSpawnDistance)
            {
                // Check if position overlaps with obstacles (tiles/ground)
                Collider2D hit = Physics2D.OverlapCircle(spawnPos, spawnCheckRadius, obstacleLayer);
                if (hit == null) // No obstacle found, position is clear
                {
                    return spawnPos;
                }
            }
        }

        return Vector2.zero; // Failed to find valid position
    }

    private void SpawnEnemy(EnemyDataSO enemyData, Vector2 position, DepthLayer layer)
    {
        if (enemyData.Prefab == null)
        {
            Debug.LogWarning($"Enemy prefab is null for {enemyData.name}");
            return;
        }

        GameObject enemyObj = Instantiate(enemyData.Prefab, position, Quaternion.identity, spawnParent);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.InitializeEnemy(enemyData);
            activeEnemies[layer].Add(enemy);
        }
        else
        {
            Debug.LogError($"Enemy component not found on prefab {enemyData.Prefab.name}");
            Destroy(enemyObj);
        }
    }

    private EnemyDataSO SelectRandomEnemy(List<EnemyDataSO> enemyPool)
    {
        if (enemyPool.Count == 0) return null;

        // Calculate total weight
        float totalWeight = 0f;
        foreach (var enemy in enemyPool)
        {
            totalWeight += enemy.SpawnWeight;
        }

        // Random selection based on weight
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var enemy in enemyPool)
        {
            currentWeight += enemy.SpawnWeight;
            if (randomValue <= currentWeight)
            {
                return enemy;
            }
        }

        return enemyPool[0]; // Fallback
    }

    private List<EnemyDataSO> GetEnemyPoolForLayer(DepthLayer layer)
    {
        return layer switch
        {
            DepthLayer.Surface => surfaceEnemies,
            DepthLayer.Shallow => shallowEnemies,
            DepthLayer.Medium => mediumEnemies,
            DepthLayer.Deep => deepEnemies,
            DepthLayer.Abyss => abyssEnemies,
            _ => null
        };
    }

    private DepthLayer GetDepthLayer(float yPosition)
    {
        if (yPosition > -20f) return DepthLayer.Surface;
        if (yPosition > -40f) return DepthLayer.Shallow;
        if (yPosition > -70f) return DepthLayer.Medium;
        if (yPosition > -100f) return DepthLayer.Deep;
        return DepthLayer.Abyss;
    }

    private (float min, float max) GetDepthRange(DepthLayer layer)
    {
        return layer switch
        {
            DepthLayer.Surface => (0f, -20f),
            DepthLayer.Shallow => (-20f, -40f),
            DepthLayer.Medium => (-40f, -70f),
            DepthLayer.Deep => (-70f, -100f),
            DepthLayer.Abyss => (-100f, -200f),
            _ => (0f, -20f)
        };
    }

    private void HandleEnemyDied(Enemy enemy)
    {
        // Remove from tracking
        foreach (var layer in activeEnemies.Keys)
        {
            activeEnemies[layer].Remove(enemy);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (playerTransform == null) return;

        // Draw spawn radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform.position, minSpawnDistance);

        // Draw world bounds
        Gizmos.color = Color.cyan;
        Vector2 center = (minSpawnBounds + maxSpawnBounds) / 2f;
        Vector2 size = maxSpawnBounds - minSpawnBounds;
        Gizmos.DrawWireCube(center, size);
    }
}
