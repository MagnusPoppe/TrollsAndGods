using System;
using OverworldObjects;
using System.Collections.Generic;

namespace MapGenerator
{
    /// <summary>
    /// The Block class was used to place out buildings.
    /// </summary>
	public class Block
	{
        Point position;
		bool[] possibleBuildings;
		float rating;
		float distanceFromCastle;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGenerator.Block"/> class.
        /// </summary>
        /// <param name="origin">Origin.</param>
        /// <param name="position">Position.</param>
        /// <param name="canWalk">Can walk.</param>
        public Block(Point origin, Point position, int[,] canWalk)
		{
			this.position = position;
			possibleBuildings = Shapes.GetBuildingFit(position, canWalk);
			rating = 0.0f;

			distanceFromCastle = origin.DistanceTo(position);

			for (int i = 0; i < possibleBuildings.Length; i++)
				if (possibleBuildings[i])
					rating++;

			rating = (float)Math.Pow(rating/13, 3)*13;
			//TODO Debug.Log("RATING: " + rating);
		}

        /// <summary>
        /// Gets the distance from castle.
        /// </summary>
        /// <returns>The distance from castle.</returns>
		public float GetDistanceFromCastle()
		{
			return distanceFromCastle;
		}

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <returns>The position.</returns>
		public Point GetPosition()
		{
			return position;
		}

        /// <summary>
        /// Gets the rating of the block.
        /// </summary>
        /// <returns>The rating.</returns>
		public float GetRating()
		{
			return rating;
		}

        /// <summary>
        /// Determines whether this instance can place building of the specified buildingType.
        /// </summary>
        /// <returns><c>true</c> if this instance can place building the specified buildingType; otherwise, <c>false</c>.</returns>
        /// <param name="buildingType">Building type.</param>
		public bool CanPlaceBuilding(int buildingType)
		{
			return possibleBuildings[buildingType];
		}

        /// <summary>
        /// Gets the occupied tiles.
        /// </summary>
        /// <returns>The occupied tiles.</returns>
        /// <param name="buildingType">Building type.</param>
        public Point[] GetOccupiedTiles(int buildingType)
		{
			int x = position.x;
			int y = position.y;

			int[,] shape = Shapes.GetShape(buildingType);
            List<Point> occupiedArea = new List<Point>();

			for (int iy = 0; iy < shape.GetLength(0); iy++)
			{
				for (int ix = 0; ix < shape.GetLength(1); ix++)
				{
					int dx = Shapes.dx[ix];
					int dy = Shapes.dy[iy];

					if (shape[ix, iy] == 1)
                        occupiedArea.Add(new Point(x + dx, y + dy));
				}
			}
			if (occupiedArea.Count > 0)
				return occupiedArea.ToArray();

			return null;
		}
		

	}
}
