using System;
using OverworldObjects;
using UnityEngine;

namespace OverworldObjects
{
	public class OreMine : ResourceBuilding
	{
		const int SHAPE = Shapes.CUBE01;
		const int SPRITE_ID = 0;
        const Resources.type RESOURCE_ID = Resources.type.WOOD;

		const int MINIMUM_PREFERED_DISTANCE_FROM_TOWN = 10;
		const int MAXIMUM_PREFERED_DISTANCE_FROM_TOWN = 15;

		public OreMine(Player owner) 
		: base(SHAPE, owner, SPRITE_ID, RESOURCE_ID, MINIMUM_PREFERED_DISTANCE_FROM_TOWN, MAXIMUM_PREFERED_DISTANCE_FROM_TOWN)
		{
			
		}
	}
}
