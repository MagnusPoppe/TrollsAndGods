using System;
using UnityEngine;
namespace MapGenerator
{
	public class TileRating
	{
		float rating;
		Vector2 position;

		public TileRating(float rating, Vector2 position)
		{
			this.rating = rating;
			this.position = position;
		}

		public float Rating
		{
			get
			{
				return rating;
			}

			set
			{
				rating = value;
			}
		}

		public Vector2 Position
		{
			get
			{
				return position;
			}

			set
			{
				position = value;
			}
		}

		//public RateTile(int x, int y, bool[,] map);
	}
}
