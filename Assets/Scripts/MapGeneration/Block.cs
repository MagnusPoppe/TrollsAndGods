using System;
using UnityEngine;
using OverworldObjects;
namespace MapGenerator
{
	public class Block
	{
		Vector2 position;
		bool[] possibleBuildings;
		float rating;
		float distanceFromCastle;

		public Block(Vector2 origin, Vector2 position, bool[,] canWalk)
		{
			this.position = position;
			possibleBuildings = OverworldShapes.GetBuildingFit(position, canWalk);
			rating = 0.0f;

			distanceFromCastle = Vector2.Distance(origin, position);

			for (int i = 0; i < possibleBuildings.Length; i++)
				if (possibleBuildings[i])
					rating++;

			rating = rating / distanceFromCastle;
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
		

	}
}
