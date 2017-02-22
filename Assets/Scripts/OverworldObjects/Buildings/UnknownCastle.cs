using System;
using OverworldObjects;
using UnityEngine;

namespace OverworldObjects
{
    class UnknownCastle : Castle
    {
        const int shape = Shapes.QUAD02x2;
        const int environmentTileType = MapGenerator.MapMaker.GRASS_SPRITEID;
        private const int LOCAL_SPRITE_ID = 0;
        const IngameObjectLibrary.Category SPRITE_CATEGORY = IngameObjectLibrary.Category.Castle;

        public UnknownCastle(Vector2 origo, Player owner ) 
            : base(origo, shape, owner, LOCAL_SPRITE_ID, SPRITE_CATEGORY, environmentTileType)
        {
			EnvironmentTileType = MapGenerator.MapMaker.GRASS_SPRITEID;
        }

        public UnknownCastle( Player owner ) 
            : base( shape, owner, LOCAL_SPRITE_ID, SPRITE_CATEGORY)
        {
        }
    }
}
