using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerator
{
    public class WaterRegion : Region
    {

        public WaterRegion(List<Vector2> coordinateList, Vector2 regionCenter) 
            :base(coordinateList, regionCenter)
        {
           
        }

        /// <summary>
        /// Fills the region with water.
        /// Replaces walkable area with water and 
        /// not walkable area turns into land (regular ground).
        /// </summary>
        /// <param name="map">Map.</param>
        public void FillRegionWithWater(int[,] map)
        {
            map[(int)RegionCenter.x, (int)RegionCenter.y] = MapMaker.GROUND;
            foreach (Vector2 pkt in coordinates)
            {
                int x = (int)pkt.x;
                int y = (int)pkt.y;

                if (map[x, y] == MapMaker.GROUND)
                {
                    map[x, y] = MapMaker.WATER_SPRITEID;

                }
                else if (map[x, y] == MapMaker.REGION_CENTER)
                {
                    map[x, y] = MapMaker.WATER_SPRITEID;
                }
                else if (map[x, y] == MapMaker.WALL)
                {
                    map[x, y] = MapMaker.GRASS_SPRITEID;
                }
            }
        }

    }
}

