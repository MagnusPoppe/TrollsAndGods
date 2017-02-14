using System;
using UnityEngine;
using OverworldObjects;

namespace Buildings
{
	public class OverworldBuilding : OverworldInteractable
	{
		protected int shapeType;
		protected int owner;
		protected int spriteID;

		private int minDistFromTown;
		private int maxDistFromTown;

		public OverworldBuilding(int shape, int owner, int spriteID, int minDistFromTown, int maxDistFromTown) 
			: base()
		{
			this.shapeType = shape;
			this.owner = owner;
			this.spriteID = spriteID;
			this.minDistFromTown = minDistFromTown;
			this.maxDistFromTown = maxDistFromTown;
		}

		public void FilpCanWalk( int[,] canWalk )
		{
			int x = (int)origo.x;
			int y = (int)origo.y;

			int[,] shape = Shapes.GetShape(shapeType);

			for (int fy = 0; fy < shape.GetLength(0); fy++)
			{
				for (int fx = 0; fx < shape.GetLength(1); fx++)
				{
					int dxx = x + Shapes.dx[fx];
					int dyy = y + Shapes.dy[fy];

					if (shape[fx,fy] == 1)
						canWalk[dxx,dyy] = MapGenerator.MapMaker.CANWALK;
				}
			}
		}

		public int GetSprite()
		{
			return spriteID;
		}

		public int GetShape()
		{
			return shapeType;
		}

		public int GetMinDistanceFromTown()
		{
			return minDistFromTown;
		}

		public int GetMaxDistanceFromTown()
		{
			return maxDistFromTown;
		}
	}
}
