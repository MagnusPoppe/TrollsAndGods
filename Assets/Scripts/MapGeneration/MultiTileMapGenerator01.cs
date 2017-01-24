using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MultiTileMapGenerator01
{

    // Map properties:
    int width;
    int height;
    int[,] map;

    int[,] medianFilter =
    {
            {1, 1, 1}, 
            {1, 1, 1},
            {1, 1, 1}
    };
    int filterSum;
    int smooth;

    // Random properties:
    string seed;

    Color[] tiles =
    {
        Color.blue,      // Lava
        Color.yellow,   // Jord
        Color.green    // Gress
    };

    // Use this for initialization
    public MultiTileMapGenerator01( int width, int height, String seed, int smooth)
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
        filterSum = GetFilterSum( medianFilter );

        for (int i = 0; i < smooth; i++)
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    map[x, y] = SmoothMap(x, y);
    }

    void RandomFillMap()
    {
        // Pesuedo random number generator:
        System.Random prng = new System.Random( seed.GetHashCode() );

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

    int SmoothMap(int initialX, int initialY)
    {
        int[] values = new int[filterSum];
        int range = medianFilter.GetLength(0) / 2;
        int valueindex = 0;

        for (int x = - range; x <=  + range; x++)
        {
            for (int y = - range; y <= + range; y++)
            {
                if ( (x+initialX >= 0 && x+initialX < width)
                    && (  y+initialY >= 0 && y+initialY < height ))
                {
                    int runlength = medianFilter[x + range, y + range];
                    for (int i = 0; i < runlength; i++)
                    {
                        values[valueindex++] = map[initialX+x, initialY+y];
                    }
                }
            }
        }
        Array.Sort(values);
        return values[values.Length / 2];
    }

    int GetFilterSum( int[,] filter)
    {
        int sum = 0;
        for (int y = 0; y < filter.GetLength(0); y++)
            for (int x = 0; x < filter.GetLength(1); x++)
                sum += filter[x, y];

        return sum;
    }
}