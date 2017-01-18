using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BinaryMapGenerator : MonoBehaviour
{

    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(35, 50)]
    public int randomFillPercent;
    int[,] map;
    public int smoothingRuns;

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GenerateMap();
    }

    /// <summary>
    /// Generates the map.
    /// </summary>
    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < smoothingRuns; i++)
        {
            SmoothMap();
        }
    }

    /// <summary>
    /// Randomly the fill map.
    /// </summary>
    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        // Pesuedo random number generator:
        System.Random prng = new System.Random(seed.GetHashCode());

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    map[x, y] = 1;
                else
                    map[x, y] = prng.Next(0, 100) < randomFillPercent ? 1 : 0;
            }
        }
    }

    /// <summary>
    /// Smooths the map.
    /// </summary>
    void SmoothMap()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int neighbourWalls = GetSurroundingWallCount(x, y);

                if (neighbourWalls < 4)      map[x, y] = 0;
                else if (neighbourWalls > 4) map[x, y] = 1;
            }
        }
    }

    /// <summary>
    /// Gets the surrounding wall count.
    /// </summary>
    /// <returns>The surrounding wall count.</returns>
    /// <param name="initX">Inital x.</param>
    /// <param name="initY">Inital y.</param>
    int GetSurroundingWallCount(int initX, int initY)
    {
        int wallCount = 0;
        for( int x = initX-1; x <= initX +1; x++) 
        {
            for ( int y = initY-1; y <= initY +1; y++)
            {
                if (x >= 0 && x < width && y >= 0 && y < height)
                {
                    if (x != initX || y != initY) 
                    {
                        wallCount += map[x, y];
                    }
                }
                else 
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    /// <summary>
    /// Raises the draw gizmos event.
    /// </summary>
    void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Gizmos.color = map[x, y] == 1 ? Color.black : Color.white;
                    Vector3 pos = new Vector3(
                        -width / 2 + x + 0.5f,
                        0,
                        -height / 2 + y + 0.5f
                    );
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
