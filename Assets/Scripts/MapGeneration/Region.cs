using System;
using System.Collections.Generic;

namespace MapGenerator
{
    /// <summary>
    /// Region. This is the parent class of all region types, and 
    /// contains the common data for regions.
    /// </summary>
	public class Region : IComparable
	{
        protected List<Point> coordinates;
        private Point regionCenter;

        /// <summary>
        /// Gets or sets the region center.
        /// </summary>
        /// <value>The region center.</value>
        public Point RegionCenter
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGenerator.Region"/> class.
        /// </summary>
        /// <param name="coordinates">Coordinates.</param>
        /// <param name="regionCenter">Region center.</param>
        public Region(List<Point> coordinates, Point regionCenter)
        {
            this.coordinates = coordinates;
            this.RegionCenter = regionCenter;
        } 
        
        /// <returns>All points in region as list</returns>
        public List<Point> GetCoordinates()
        {
            return coordinates;
        }

        /// <returns>All points in region as array</returns>
        public Point[] GetCoordinatesArray()
        {
            int i = 0;
            Point[] temp = new Point[coordinates.Count];
            foreach (Point c in coordinates)
                coordinates[i++] = c;
            return temp;
        }



        /// <returns>X value of the region center point</returns>
        public int getX()
        {
            return RegionCenter.x;
        }

        /// <returns>Y value of the region center point</returns>
        public int getY()
        {
            return RegionCenter.y;
        }

        /// <summary>
        /// Adds a point to region.
        /// </summary>
        /// <param name="pkt">point to be added.</param>
        public void AddToRegion(Point pkt)
        {
            coordinates.Add(pkt);
        }
        
        /// <summary>
        /// Takes a Point and checks if its is in region.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool isPointInRegion(Point point)
        {
            foreach (Point p in coordinates)
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
            foreach (Point v in coordinates)
            {
                if (map[v.x,v.y] >= RegionFill.DEFAULT_LABEL_START)
                    map[v.x,v.y] = MapMaker.GROUND;
            }
            return map;
        }

        /// <summary>
        /// Default CompareTo after the IComparable Interface. Compares
        /// regions based on size.
        /// </summary>
        /// <returns>The to.</returns>
        /// <param name="obj">Object.</param>
        public int CompareTo(object obj)
        {
            Region other = (Region)obj;
            return GetArea() - other.GetArea();
        }

        /// <summary>
        /// Determines whether the specified <see cref="MapGenerator.Region"/> is equal to the current <see cref="MapGenerator.Region"/>.
        /// </summary>
        /// <param name="other">The <see cref="MapGenerator.Region"/> to compare with the current <see cref="MapGenerator.Region"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="MapGenerator.Region"/> is equal to the current
        /// <see cref="MapGenerator.Region"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(Region other)
        {
            return getX() == other.getX() && getY() == other.getY();
        }
    }
}