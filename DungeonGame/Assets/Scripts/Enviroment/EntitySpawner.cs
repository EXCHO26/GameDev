using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemySpawnConfig
{
    public string enemyName;
    public GameObject prefab;
    public int poolSize = 20;
    public int spawnWeight = 10;
}

public struct PendingSpawn
{
    public Vector2 position;
    public EnemySpawnConfig config;
}

public class EntitySpawner : MonoBehaviour
{
    [Header("References")]
    public CaveTilemapGenerator mapGen;
    public Transform player;

    [Header("Settings")]
    [Range(0, 0.1f)] public float enemyDensity = 0.02f;
    public float spawnDistance = 15f;
    public float despawnDistance = 25f;

    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public int bossClearRadius = 3;

    [Header("Enemies")]
    public List<EnemySpawnConfig> enemyTypes;

    private List<PendingSpawn> pendingSpawns = new List<PendingSpawn>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();

    private void Start()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var enemy in enemyTypes)
        {
            Queue<GameObject> newPool = new Queue<GameObject>();
            for (int i = 0; i < enemy.poolSize; i++)
            {
                GameObject obj = Instantiate(enemy.prefab, transform);
                obj.SetActive(false);
                newPool.Enqueue(obj);
            }
            pools.Add(enemy.prefab, newPool);
        }
    }

    public void PopulateCave(int[,] map, int width, int height)
    {
        TrySpawnBoss(map, width, height);

        List<Vector2Int> validTiles = GetValidSpawnPositions(map, width, height);
        
        int spawnCount = Mathf.RoundToInt(validTiles.Count * enemyDensity);
        ScheduleSpawns(validTiles, spawnCount);
    }

    private void TrySpawnBoss(int[,] map, int width, int height)
    {
        if (bossPrefab == null) return;

        List<Vector2Int> validBossLocations = new List<Vector2Int>();

        for (int x = bossClearRadius; x < width - bossClearRadius; x++)
        {
            for (int y = bossClearRadius; y < height - bossClearRadius; y++)
            {
                if (IsAreaClear(map, x, y, bossClearRadius))
                {
                    validBossLocations.Add(new Vector2Int(x, y));
                }
            }
        }

        if (validBossLocations.Count > 0)
        {
            int randomIndex = Random.Range(0, validBossLocations.Count);
            Vector2Int bossCoord = validBossLocations[randomIndex];
            
            Vector3 worldPos = mapGen.wallTilemap.GetCellCenterWorld(new Vector3Int(bossCoord.x, bossCoord.y, 0));
            
            Instantiate(bossPrefab, worldPos, Quaternion.identity);
        }
    }

    private bool IsAreaClear(int[,] map, int centerX, int centerY, int radius)
    {
        for (int x = centerX - radius; x <= centerX + radius; x++)
        {
            for (int y = centerY - radius; y <= centerY + radius; y++)
            {
                if (map[x, y] != 0) 
                {
                    return false;
                }
            }
        }
        return true;
    }

    private List<Vector2Int> GetValidSpawnPositions(int[,] map, int width, int height)
    {
        List<Vector2Int> validTiles = new List<Vector2Int>();
        Vector2 center = new Vector2(width / 2f, height / 2f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0 && Vector2.Distance(new Vector2(x, y), center) > mapGen.spawnRadius + 2)
                {
                    validTiles.Add(new Vector2Int(x, y));
                }
            }
        }
        return validTiles;
    }

    private void ScheduleSpawns(List<Vector2Int> validTiles, int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (validTiles.Count == 0) break;

            int randomIndex = Random.Range(0, validTiles.Count);
            Vector2Int coord = validTiles[randomIndex];
            
            Vector3 worldPos = mapGen.wallTilemap.GetCellCenterWorld(new Vector3Int(coord.x, coord.y, 0));
            
            pendingSpawns.Add(new PendingSpawn 
            { 
                position = new Vector2(worldPos.x, worldPos.y), 
                config = GetRandomEnemyType() 
            });

            validTiles.RemoveAt(randomIndex);
        }
    }

    private void Update()
    {
        if (player == null) return;

        ProcessPendingSpawns();
        ProcessActiveEnemies();
    }

    private void ProcessPendingSpawns()
    {
        Vector2 playerPos = player.position;

        for (int i = pendingSpawns.Count - 1; i >= 0; i--)
        {
            if (Vector2.Distance(playerPos, pendingSpawns[i].position) <= spawnDistance)
            {
                SpawnEnemy(pendingSpawns[i]);
                pendingSpawns.RemoveAt(i);
            }
        }
    }

    private void ProcessActiveEnemies()
    {
        Vector2 playerPos = player.position;

        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = activeEnemies[i];

            if (IsEnemyInvalid(enemy))
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            if (ShouldDespawn(enemy, playerPos))
            {
                DespawnEnemy(enemy, i);
            }
        }
    }
    private bool IsEnemyInvalid(GameObject enemy)
    {
        return enemy == null || !enemy.activeSelf;
    }

    private bool ShouldDespawn(GameObject enemy, Vector2 playerPos)
    {
        return Vector2.Distance(playerPos, enemy.transform.position) > despawnDistance;
    }

    private void DespawnEnemy(GameObject enemy, int index)
    {
        var death = enemy.GetComponent<EnemyDeath>();
        ReturnEnemyToPool(enemy, death.prefabOrigin);
        
        pendingSpawns.Add(new PendingSpawn 
        { 
            position = enemy.transform.position, 
            config = GetConfigForPrefab(death.prefabOrigin) 
        });
        
        activeEnemies.RemoveAt(index);
    }

    private void SpawnEnemy(PendingSpawn spawnData)
    {
        if (!pools.ContainsKey(spawnData.config.prefab) || pools[spawnData.config.prefab].Count == 0) return;

        GameObject enemy = pools[spawnData.config.prefab].Dequeue();
        enemy.transform.position = spawnData.position;
        
        var death = enemy.GetComponent<EnemyDeath>();
        if (death)
        {
            death.spawner = this;
            death.prefabOrigin = spawnData.config.prefab;

            death.ResetEnemy();
        }

        enemy.SetActive(true);
        activeEnemies.Add(enemy);
    }

    public void ReturnEnemyToPool(GameObject enemy, GameObject prefabOrigin)
    {
        enemy.SetActive(false);
        enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, -1f);

        if (pools.ContainsKey(prefabOrigin))
        {
            pools[prefabOrigin].Enqueue(enemy);
        }
    }

    private EnemySpawnConfig GetRandomEnemyType()
    {
        int totalWeight = 0;
        foreach (var enemy in enemyTypes) totalWeight += enemy.spawnWeight;
        int randomValue = Random.Range(0, totalWeight);
        foreach (var enemy in enemyTypes)
        {
            if (randomValue < enemy.spawnWeight) return enemy;
            randomValue -= enemy.spawnWeight;
        }
        return enemyTypes[0];
    }

    private EnemySpawnConfig GetConfigForPrefab(GameObject prefab)
    {
        foreach (var config in enemyTypes)
        {
            if (config.prefab == prefab) return config;
        }
        return enemyTypes[0];
    }
}