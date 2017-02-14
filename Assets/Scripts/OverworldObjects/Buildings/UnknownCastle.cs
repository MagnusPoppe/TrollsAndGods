using System;
using OverworldObjects;
using UnityEngine;

namespace Buildings
{
    class UnknownCastle : Castle
    {

        const int shape = Shapes.QUAD02x2;
        const int spriteID = MapGenerator.MapMaker.CASTLE;
        const int environmentTileType = MapGenerator.MapMaker.GROUND;

        public UnknownCastle(Vector2 origo, int owner ) 
            : base(origo, shape, owner, spriteID, environmentTileType)
        {
			EnvironmentTileType = MapGenerator.MapMaker.GROUND;
        }

        public UnknownCastle( int owner ) 
            : base( shape, owner, spriteID)
        {
        }
    }
}
