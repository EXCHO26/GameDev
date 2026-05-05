using System.Collections;
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

    [Header("Tilemap")]
    public Tilemap wallTilemap;
    public Tilemap floorTilemap;

    public TileBase wallTile;
    public TileBase floorTile;

    [Header("Cleanup")]
    public int wallThresholdSize = 50;
    public int roomThresholdSize = 50;

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

        ProcessMap();

        DrawMap();
    }

    void RandomFillMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    map[x, y] = 1;
                else
                    map[x, y] = (Random.Range(0, 100) < randomFillPercent) ? 1 : 0;
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
                int neighbours = GetSurroundingWallCount(x, y);

                if (neighbours > 4)
                    newMap[x, y] = 1;
                else if (neighbours < 4)
                    newMap[x, y] = 0;
                else
                    newMap[x, y] = map[x, y];
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
                if (x >= 0 && x < width && y >= 0 && y < height)
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

    void DrawMap()
    {
        wallTilemap.ClearAllTiles();
        floorTilemap.ClearAllTiles();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                }
            }
        }
    }

    void ProcessMap()
    {
        // махане на малки wall islands
        List<List<Vector2Int>> wallRegions = GetRegions(1);

        foreach (var region in wallRegions)
        {
            if (region.Count < wallThresholdSize)
            {
                foreach (var tile in region)
                    map[tile.x, tile.y] = 0;
            }
        }

        // махане на малки room regions
        List<List<Vector2Int>> roomRegions = GetRegions(0);

        foreach (var region in roomRegions)
        {
            if (region.Count < roomThresholdSize)
            {
                foreach (var tile in region)
                    map[tile.x, tile.y] = 1;
            }
        }
    }

    List<List<Vector2Int>> GetRegions(int tileType)
    {
        List<List<Vector2Int>> regions = new List<List<Vector2Int>>();
        bool[,] visited = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!visited[x, y] && map[x, y] == tileType)
                {
                    List<Vector2Int> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (var tile in newRegion)
                        visited[tile.x, tile.y] = true;
                }
            }
        }

        return regions;
    }

    List<Vector2Int> GetRegionTiles(int startX, int startY)
    {
        List<Vector2Int> tiles = new List<Vector2Int>();
        bool[,] visited = new bool[width, height];

        int tileType = map[startX, startY];

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(startX, startY));
        visited[startX, startY] = true;

        while (queue.Count > 0)
        {
            Vector2Int tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.x - 1; x <= tile.x + 1; x++)
            {
                for (int y = tile.y - 1; y <= tile.y + 1; y++)
                {
                    if (IsInMap(x, y) && !visited[x, y])
                    {
                        // само 4-direction (без диагонали)
                        if ((x == tile.x || y == tile.y) && map[x, y] == tileType)
                        {
                            visited[x, y] = true;
                            queue.Enqueue(new Vector2Int(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }

    bool IsInMap(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}