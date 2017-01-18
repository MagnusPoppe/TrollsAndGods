using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MultiTileMapGenerator : MonoBehaviour 
{
    // Map properties:
    int width;
    int height;
    int[,] map;

    int smooth;

    // Random properties:
    string seed;
    Color[] tiles =
        {
            Color.red,      // Lava
            Color.yellow,   // Jord
            Color.green,    // Gress
        };

    // Use this for initialization
    public MultiTileMapGenerator( int width, int height, string seed, int smooth)
    {
        this.width = width;
        this.height = height;
        this.seed = seed;
        this.smooth = smooth;
        GenerateMap();  
    }

    public int[,] GetMap()
    {
        return map;
    }

    void GenerateMap()
    {
        // Creating a random map:
        map = new int[width, height];
        RandomFillMap();

        // Smoothing the map to get islands:
        int[,] defaultFilter =
        {
            { 1, 1, 1 }, 
            { 1, 1, 1 },
            { 1, 1, 1 }
        };
        MedianFilter filter = new MedianFilter(defaultFilter);

        for (int i = 0; i < smooth; i++)
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    map[x, y] = filter.Apply(x, y, map);
    }

    void RandomFillMap()
    {
        // Pesuedo random number generator:
        System.Random prng = new System.Random(seed.GetHashCode());

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    map[x, y] = 1;
                else
                    map[x, y] = prng.Next(0, tiles.Length);
            }
        }
    }
}
