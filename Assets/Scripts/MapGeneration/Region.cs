using System;
using UnityEngine;
using System.Collections.Generic;
using OverworldObjects;

namespace MapGenerator
{
	public class Region : IComparable
	{
        protected List<Vector2> coordinates;
        private Vector2 regionCenter;

        public Vector2 RegionCenter
        {
            get
            {
                return regionCenter;
            }

            set
            {
                regionCenter = value;
            }
        }

        public Region(List<Vector2> coordinates, Vector2 regionCenter)
        {
            this.coordinates = coordinates;
            this.RegionCenter = regionCenter;
        } 
        
        /// <returns>All points in region as list</returns>
        public List<Vector2> GetCoordinates()
        {
            return coordinates;
        }

        /// <returns>All points in region as array</returns>
		public Vector2[] GetCoordinatesArray()
        {
            int i = 0;
            Vector2[] temp = new Vector2[coordinates.Count];
            foreach (Vector2 c in coordinates)
                coordinates[i++] = c;
            return temp;
        }



        /// <returns>X value of the region center point</returns>
        public int getX()
        {
            return (int) RegionCenter.x;
        }

        /// <returns>Y value of the region center point</returns>
        public int getY()
        {
            return (int)RegionCenter.y;
        }

        /// <summary>
        /// Adds a point to region.
        /// </summary>
        /// <param name="pkt">point to be added.</param>
        public void AddToRegion(Vector2 pkt)
        {
            coordinates.Add(pkt);
        }
        
        /// <summary>
        /// Takes a vector2 and checks if its is in region.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool isPointInRegion(Vector2 point)
        {
            foreach (Vector2 p in coordinates)
            {
                if (p.x == point.x && p.y == point.y)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the area of the region. (AREAL)
        /// </summary>
        /// <returns>The area.</returns>
        public int GetArea()
        {
            return coordinates.Count;
        }

        /// <summary>
        /// Resets the type of groundtile to the standard ground
        /// tile. This includes all tiles bigger than the reserved
        /// numbers.
        /// Definition of groundtile is found in MapMaker.GROUND
        /// </summary>
        /// <returns>Reset map</returns>
        /// <param name="map">Map.</param>
        public int[,] ResetRegionGroundTileType(int[,] map)
        {
            foreach (Vector2 v in coordinates)
            {
                if (map[(int)v.x, (int)v.y] >= RegionFill.DEFAULT_LABEL_START)
                    map[(int)v.x, (int)v.y] = MapMaker.GROUND;
            }
            return map;
        }



        /// <summary>
        /// Sets all ground tiles to a given type of tile.
        /// Definition of groundtile is found in MapMaker.GROUND
        /// </summary>
        /// <returns>The region ground tile type.</returns>
        /// <param name="groundTile">Ground tile.</param>
        /// <param name="map">Map.</param>
        public int[,] SetRegionGroundTileType(int groundTile, int[,] map)
        {
            foreach (Vector2 v in coordinates)
            {
                if (map[(int)v.x, (int)v.y] == MapMaker.GROUND)
                    map[(int)v.x, (int)v.y] = groundTile;
            }
            return map;
        }

        public int CompareTo(object obj)
        {
            Region other = (Region)obj;
            return GetArea() - other.GetArea();
        }

        public bool Equals(Region other)
        {
            return getX() == other.getX() && getY() == other.getY();
        }
    }
}