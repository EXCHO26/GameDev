using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CaveTilemapGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public int width = 100;
    public int height = 100;

    [Range(0, 100)]
    public int randomFillPercent = 45;

    public int smoothIterations = 5;

    [Header("Tilemap References")]
    public Tilemap wallTilemap;
    public Tilemap floorTilemap;

    public TileBase wallTile;
    public TileBase floorTile;

    [Header("Cleanup Settings")]
    public int wallThresholdSize = 50;
    public int roomThresholdSize = 50;

    [Header("Spawn Settings")]
    public int spawnRadius = 3;
    public GameObject playerSpawner;
    public GameObject player;
    public EntitySpawner entitySpawner;

    private int[,] map;

    void Start()
    {
        GenerateMap();
    }
    
    public void GenerateMap()
    {
        map = new int[width, height];
        
        RandomFillMap();

        for (int i = 0; i < smoothIterations; i++)
        {
            SmoothMap();
        }

        ClearCenter();
        ProcessMap();
        DrawMap();

        PlacePlayer();

        if(entitySpawner)
        {
            entitySpawner.PopulateCave(map, width, height);
        }
    }

    void RandomFillMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < 2 || x >= width - 2 || y < 2 || y >= height - 2)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (Random.Range(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        int[,] newMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbors = GetSurroundingWallCount(x, y);

                if (neighbors > 4) newMap[x, y] = 1;
                else if (neighbors < 4) newMap[x, y] = 0;
                else newMap[x, y] = map[x, y];
            }
        }
        map = newMap;
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int count = 0;
        for (int x = gridX - 1; x <= gridX + 1; x++)
        {
            for (int y = gridY - 1; y <= gridY + 1; y++)
            {
                if (IsInMap(x, y))
                {
                    if (x != gridX || y != gridY)
                        count += map[x, y];
                }
                else
                {
                    count++;
                }
            }
        }
        return count;
    }

    void ProcessMap()
    {
        bool[,] visited = new bool[width, height];

        CleanupRegions(1, wallThresholdSize, 0, visited);
        
        System.Array.Clear(visited, 0, visited.Length);

        CleanupRegions(0, roomThresholdSize, 1, visited);

        RemovePointedEdges();
    }

    void CleanupRegions(int tileType, int threshold, int replacementType, bool[,] visited)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!visited[x, y] && map[x, y] == tileType)
                {
                    List<Vector2Int> region = GetRegionTiles(x, y, visited);
                    if (region.Count < threshold)
                    {
                        foreach (Vector2Int tile in region)
                        {
                            map[tile.x, tile.y] = replacementType;
                        }
                    }
                }
            }
        }
    }

    List<Vector2Int> GetRegionTiles(int startX, int startY, bool[,] visited)
    {
        List<Vector2Int> tiles = new List<Vector2Int>();
        int tileType = map[startX, startY];

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        while (queue.Count > 0)
        {
            Vector2Int tile = queue.Dequeue();
            tiles.Add(tile);

            int[] dx = { 0, 0, 1, -1 };
            int[] dy = { 1, -1, 0, 0 };

            for (int i = 0; i < 4; i++)
            {
                int nx = tile.x + dx[i];
                int ny = tile.y + dy[i];

                if (IsInMap(nx, ny) && !visited[nx, ny] && map[nx, ny] == tileType)
                {
                    visited[nx, ny] = true;
                    queue.Enqueue(new Vector2Int(nx, ny));
                }
            }
        }
        return tiles;
    }

    void RemovePointedEdges()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (map[x, y] == 1)
                    {
                        int neighbors = 0;
                        if (map[x + 1, y] == 1) neighbors++;
                        if (map[x - 1, y] == 1) neighbors++;
                        if (map[x, y + 1] == 1) neighbors++;
                        if (map[x, y - 1] == 1) neighbors++;

                        if (neighbors <= 1) map[x, y] = 0;
                    }
                }
            }
        }
    }

    void DrawMap()
    {
        wallTilemap.ClearAllTiles();
        floorTilemap.ClearAllTiles();

        Vector3Int[] positions = new Vector3Int[width * height];
        TileBase[] wallTiles = new TileBase[width * height];
        TileBase[] floorTiles = new TileBase[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = x + y * width;
                positions[index] = new Vector3Int(x, y, 0);
                floorTiles[index] = floorTile;
                wallTiles[index] = (map[x, y] == 1) ? wallTile : null;
            }
        }

        floorTilemap.SetTiles(positions, floorTiles);
        wallTilemap.SetTiles(positions, wallTiles);
    }

    void ClearCenter()
    {
        int centerX = width / 2;
        int centerY = height / 2;

        for (int x = centerX - spawnRadius; x <= centerX + spawnRadius; x++)
        {
            for (int y = centerY - spawnRadius; y <= centerY + spawnRadius; y++)
            {
                if (IsInMap(x, y))
                {
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                    if (dist <= spawnRadius)
                    {
                        map[x, y] = 0;
                    }
                }
            }
        }
    }

    void PlacePlayer()
    {
        if (player)
        {
            Vector3Int centerCell = new Vector3Int(width / 2, height / 2, 0);
            Vector3Int playerCell = new Vector3Int(width / 2, height / 2 + 1, 0);

            player.transform.position = wallTilemap.GetCellCenterWorld(playerCell);
        }
    }

    bool IsInMap(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}