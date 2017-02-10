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

		public OverworldBuilding(Vector2 center, int shape, int owner, int spriteID) : base(center)
		{
			this.shapeType = shape;
			this.owner = owner;
			this.spriteID = spriteID;
		}

		protected void FilpCanWalk( bool[,] canWalk )
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
						canWalk[dxx,dyy] = false;
				}
			}
		}


	}
}
