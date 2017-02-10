using System;
using UnityEngine;
using OverworldObjects;
using System.Collections.Generic;

namespace MapGenerator
{
	public class Block
	{
		Vector2 position;
		bool[] possibleBuildings;
		float rating;
		float distanceFromCastle;

		public Block(Vector2 origin, Vector2 position, int[,] canWalk)
		{
			this.position = position;
			possibleBuildings = Shapes.GetBuildingFit(position, canWalk);
			rating = 0.0f;

			distanceFromCastle = Vector2.Distance(origin, position);

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

		public Vector2 GetPosition()
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

		public Vector2[] GetOccupiedTiles(int buildingType)
		{
			int x = (int)position.x;
			int y = (int)position.y;

			int[,] shape = Shapes.GetShape(buildingType);
			List<Vector2> occupiedArea = new List<Vector2>();

			for (int iy = 0; iy < shape.GetLength(0); iy++)
			{
				for (int ix = 0; ix < shape.GetLength(1); ix++)
				{
					int dx = Shapes.dx[ix];
					int dy = Shapes.dy[iy];

					if (shape[ix, iy] == 1)
						occupiedArea.Add(new Vector2(x + dx, y + dy));
				}
			}
			if (occupiedArea.Count > 0)
				return occupiedArea.ToArray();

			return null;
		}
		

	}
}
