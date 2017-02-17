using System;
using OverworldObjects;
using UnityEngine;

namespace OverworldObjects
{
	public class OreMine : ResourceBuilding
	{
		const int SHAPE = Shapes.CUBE01;
        private const int LOCAL_SPRITE_ID = 0;
        const IngameObjectLibrary.Category SPRITE_CATEGORY = IngameObjectLibrary.Category.ResourceBuilding;
        const Resources.type RESOURCE_ID = Resources.type.ORE;
        
		const int MINIMUM_PREFERED_DISTANCE_FROM_TOWN = 10;
		const int MAXIMUM_PREFERED_DISTANCE_FROM_TOWN = 15;

		public OreMine(Player owner) 
		: base(SHAPE, owner, LOCAL_SPRITE_ID, SPRITE_CATEGORY, RESOURCE_ID, MINIMUM_PREFERED_DISTANCE_FROM_TOWN, MAXIMUM_PREFERED_DISTANCE_FROM_TOWN)
		{
			
		}


	}
}
