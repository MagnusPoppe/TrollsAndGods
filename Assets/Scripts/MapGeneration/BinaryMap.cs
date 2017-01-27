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
        int[,] map;


        /// <summary>
        /// Primary constructor. 
        /// </summary>
        public BinaryMap(int width, int height, int smoothItr, string seed, int fill, Region[] regions)
        {
            // Setting variables:
            this.width = width;
            this.height = height;

            map = new int[width, height];
			RandomFillMap(seed, fill);

			DefineCenterOfRegions(regions, map);

            for (int i = 0; i < smoothItr; i++)
            {
                SmoothMap();
            }
        }

		/// <summary>
		/// Gets the map.
		/// </summary>
		/// <returns>The map.</returns>
        public int[,] getMap()
        {
            return map;
        }

		/// <summary>
		/// Defines the center of regions by filling the ground 
		/// around them
		/// </summary>
		/// <param name="regions">Regions.</param>
		/// <param name="thisMap">This map.</param>
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
        void RandomFillMap(string seed, int randomFillPercent)
        {
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
					copy[x,y] = filter.Apply(x, y, map);
                }
            }
            map = copy;
        }

		/// <summary>
		/// Copies the map so that its not reference transferred.
		/// </summary>
		/// <returns>A copy of the map.</returns>
        int[,] copyMap()
        {
            int[,] copy = new int[width, height];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    copy[x,y] = map[x, y];

            return copy;
        }
    }
}

