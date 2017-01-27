using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MapGenerator
{

    public class BinaryMap
    {

        int width;
        int height;

        string seed;
        bool useRandomSeed;

        [Range(35, 50)]
        int randomFillPercent;
        int[,] map;


        /// <summary>
        /// Primary constructor. 
        /// </summary>
        public BinaryMap(int width, int height, int smoothItr, string seed, int fill, Region[] regions)
        {
            // Setting variables:
            this.width = width;
            this.height = height;
            this.seed = seed;

            this.randomFillPercent = fill;
              

            map = new int[width, height];
            RandomFillMap();

			DefineCenterOfRegions(regions, map);

            for (int i = 0; i < smoothItr; i++)
            {
                SmoothMap();
            }
        }

        public int[,] getMap()
        {
            return map;
        }
		void DefineCenterOfRegions(Region[] regions, int[,] thisMap)
		{
			foreach (Region r in regions)
			{
				Vector2 center = r.GetCastle().GetPosition();
				int x = (int)center.x;
				int y = (int)center.y;

				thisMap[x - 1, y] 		= MapMaker.GROUND;
				thisMap[x + 1, y] 		= MapMaker.GROUND;
				thisMap[x, y - 1] 		= MapMaker.GROUND;
				thisMap[x, y + 1] 		= MapMaker.GROUND;
				thisMap[x - 1, y - 1] 	= MapMaker.GROUND;
				thisMap[x + 1, y + 1] 	= MapMaker.GROUND;
				thisMap[x + 1, y - 1] 	= MapMaker.GROUND;
				thisMap[x - 1, y + 1] 	= MapMaker.GROUND;

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
					map[x, y] = prng.Next(0, 100) < randomFillPercent ? MapMaker.GROUND : MapMaker.WALL;
                }
            }
        }

        /// <summary>
        /// Smooths the map.
        /// </summary>
        void SmoothMap()
        {
            AverageFilter filter = new AverageFilter();
//            MedianFilter filter = new MedianFilter();
            int[,] copy = copyMap();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
//                    int neighbourWalls = GetSurroundingWallCount(x, y);
//
//                    if (neighbourWalls < 4)      map[x, y] = 0;
//                    else if (neighbourWalls > 4) map[x, y] = 1;

                    copy[x,y] = filter.Apply(x, y, map);
                }
            }
            map = copy;
        }

        int[,] copyMap()
        {
            int[,] copy = new int[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    copy[x,y] = map[x, y];

            return copy;
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
    }
}

