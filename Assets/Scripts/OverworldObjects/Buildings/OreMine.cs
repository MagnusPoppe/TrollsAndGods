using System;
using OverworldObjects;
using UnityEngine;

namespace Buildings
{
	public class OreMine : OverworldBuilding
	{
		const int SHAPE = Shapes.CUBE01;
		const int SPRITE_ID = 16;

		const int MINIMUM_PREFERED_DISTANCE_FROM_TOWN = 10;
		const int MAXIMUM_PREFERED_DISTANCE_FROM_TOWN = 15;

		public OreMine(int owner) 
		: base(SHAPE, owner, SPRITE_ID, MINIMUM_PREFERED_DISTANCE_FROM_TOWN, MAXIMUM_PREFERED_DISTANCE_FROM_TOWN)
		{
			
		}
	}
}
