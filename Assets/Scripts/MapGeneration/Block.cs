using System;
using UnityEngine;
using OverworldObjects;
using System.Collections.Generic;

namespace MapGenerator
{
	public class Block
	{
        Point position;
		bool[] possibleBuildings;
		float rating;
		float distanceFromCastle;

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

		public float GetDistanceFromCastle()
		{
			return distanceFromCastle;
		}

		public Point GetPosition()
		{
			return position;
		}

		public float GetRating()
		{
			return rating;
		}

		public bool CanPlaceBuilding(int buildingType)
		{
			return possibleBuildings[buildingType];
		}

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
