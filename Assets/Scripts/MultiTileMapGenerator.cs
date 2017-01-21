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
    int tilecount;

    // Use this for initialization
    public MultiTileMapGenerator( int width, int height, string seed, int smooth, int tilecount)
    {
        this.width = width;
        this.height = height;
        this.seed = seed;
        this.smooth = smooth;
        this.tilecount = tilecount;
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
        int[,] guassianFilter = {
            {0,1,2,1,0},
            {1,3,5,3,1},
            {2,5,9,5,2},
            {1,3,5,3,1},
            {0,1,2,1,0}
        };

        //MedianFilter filter = new MedianFilter();
        AverageFilter filter = new AverageFilter();

        for (int i = 0; i < smooth; i++)
        {
            int[,] mapCopy = copyMap(map);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    mapCopy[x, y] = filter.Apply(x, y, map);
            
            map = mapCopy;
        }
            
    }

    int[,] copyMap( int[,] map )
    {
        int[,] copy = new int[width,height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                copy[x, y] = map[x, y];
        return copy;
    }

    void RandomFillMap()
    {
        // Pesuedo random number generator:
        System.Random prng = new System.Random(seed.GetHashCode());

        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                map[x, y] = prng.Next(0, tilecount);
    }
}
