using System.Collections.Generic;

namespace MapGenerator
{
    public class WaterRegion : Region
    {

        public WaterRegion(List<Point> coordinateList, Point regionCenter) 
            :base(coordinateList, regionCenter)
        {
           
        }

        /// <summary>
        /// Fills the region with water.
        /// Replaces walkable area with water and 
        /// not walkable area turns into land (regular ground).
        /// </summary>
        /// <param name="map">Map.</param>
        public void FillRegionWithWater(int[,] map, int[,] canWalk)
        {
            map[RegionCenter.x, RegionCenter.y] = MapMaker.GROUND;
            foreach (Point pkt in coordinates)
            {
                if (map[pkt.x, pkt.y] == MapMaker.GROUND)
                {
                    map[pkt.x, pkt.y] = MapMaker.WATER_SPRITEID;
                    canWalk[pkt.x, pkt.y] = MapMaker.CANNOTWALK;
                }
                else if (map[pkt.x, pkt.y] == MapMaker.REGION_CENTER)
                {
                    map[pkt.x, pkt.y] = MapMaker.WATER_SPRITEID;
                    canWalk[pkt.x, pkt.y] = MapMaker.CANNOTWALK;
                }
                else if (map[pkt.x, pkt.y] == MapMaker.WALL)
                {
                    canWalk[pkt.x, pkt.y] = MapMaker.CANWALK;
                    map[pkt.x, pkt.y] = MapMaker.GRASS_SPRITEID;
                }
            }
        }

    }
}

