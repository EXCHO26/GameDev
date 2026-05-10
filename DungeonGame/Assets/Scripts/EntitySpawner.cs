using UnityEngine;
using System.Collections.Generic;

public class EntitySpawner : MonoBehaviour
{
    [Header("References")]
    public CaveTilemapGenerator mapGen;
    public GameObject skeletonPrefab;

    [Header("Settings")]
    [Range(0, 0.1f)] public float enemyDensity = 0.02f;

    public void PopulateCave(int[,] map, int width, int height)
    {
        List<Vector2Int> floorTiles = new List<Vector2Int>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0)
                {
                    float distToCenter = Vector2.Distance(new Vector2(x, y), new Vector2(width / 2, height / 2));
                    if (distToCenter > mapGen.spawnRadius + 2)
                    {
                        floorTiles.Add(new Vector2Int(x, y));
                    }
                }
            }
        }

        int spawnCount = Mathf.RoundToInt(floorTiles.Count * enemyDensity);
        for (int i = 0; i < spawnCount; i++)
        {
            if (floorTiles.Count == 0) break;

            int randomIndex = Random.Range(0, floorTiles.Count);
            Vector2Int coord = floorTiles[randomIndex];
            
            Vector3 spawnPos = mapGen.wallTilemap.GetCellCenterWorld(new Vector3Int(coord.x, coord.y, 0));
            
            GameObject skeleton = Instantiate(skeletonPrefab, spawnPos, Quaternion.identity);
            
            if (skeleton.TryGetComponent<SkeletonController>(out var controller))
            {
                controller.player = mapGen.player.transform;
            }
                
            floorTiles.RemoveAt(randomIndex);
        }
    }
}