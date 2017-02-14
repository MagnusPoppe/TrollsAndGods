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
            int color = UnityEngine.Random.Range(3, 15);

            EnvironmentTileType = color;
        }

        public UnknownCastle( int owner ) 
            : base( shape, owner, spriteID)
        {
        }
    }
}
