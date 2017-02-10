using System;
using OverworldObjects;
using UnityEngine;

namespace Buildings
{
	public class OreMine : OverworldBuilding
	{
		const int SHAPE = Shapes.CUBE01;
		const int SPRITE_ID = 16;

		public OreMine(Vector2 center, int owner, bool[,] canWalk) 
		: base(center, SHAPE, owner, SPRITE_ID)
		{
			FilpCanWalk(canWalk);
		}
	}
}
