using System;
using UnityEngine;

public class IngameObjectLibrary
{
    // CONSTANTS:
    const int ENVIROMENT_TYPES = 8;
    const int OBJECT_TYPES     = 50;

	// OBJECT TYPES (ARRAY 1-DIMENTION):
	const int TILE_TYPE = 0;
	const int BUILDING_TYPE_RESOURCE = 1;
	const int BUILDING_TYPE_DWELLING = 2;
	const int BUILDING_TYPE_TOWN = 3;
	const int BUILDING_TYPE_MISC = 4;
	const int PICKUP_TYPE_RESOURCE = 5;
	const int PICKUP_TYPE_ARTIFACT = 6;

    // TILETYPES (ARRAY 2-DIMENTION):
    const int ENVIROMENT_NEUTRAL        = 0;
    const int ENVIROMENT_DIRT           = 1;
    const int ENVIROMENT_GRASS          = 2;
    const int ENVIROMENT_LAVA           = 3;
    const int ENVIROMENT_WATER          = 4;
    const int ENVIROMENT_DARKNESS       = 5;

    // TILE TYPES (ARRAY 3-DIMENTION):
    const int NORTH_EDGE     = 0;
    const int NORTHEAST_EDGE = 1;
    const int EAST_EDGE      = 2;
    const int SOUTHEAST_EDGE = 3;
    const int SOUTH_EDGE     = 4;
    const int SOUTHWEST_EDGE = 5;
    const int WEST_EDGE      = 6;
    const int NORTHWEST_EDGE = 7;

	Sprite[] tiles;					
	const int TILE_START = 3;
	const int TOTAL_TILE_COUNT = 3;

	Sprite[] buildings_overworld;
    //const int BUILDING_OVERWORLD_START = TILE_START + TOTAL_TILE_COUNT;
    const int BUILDING_OVERWORLD_START = 6;
	const int TOTAL_BUILDING_OVERWORLD_COUNT = 2;



	Sprite[] buildings_town;
				

    public IngameObjectLibrary()
    {
		tiles = InitializeTiles();
		buildings_overworld = InitializeBuildings();
	}

    public Sprite GetTile(int spriteID)
    {
        return tiles[spriteID-TILE_START];
	}

	private Sprite[] InitializeTiles()
	{
		Sprite[] sprites = new Sprite[TOTAL_TILE_COUNT];
		String path = "Sprites/Tiles/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Water/Water");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Grass/Grass");
		sprites[2] = UnityEngine.Resources.Load<Sprite>(path + "Placeholder/Forest");

        return sprites;
	}

	public Sprite GetBuilding(int spriteID)
	{
		return buildings_overworld[spriteID-BUILDING_OVERWORLD_START];
	}

	private Sprite[] InitializeBuildings()
	{
		Sprite[] sprites = new Sprite[TOTAL_BUILDING_OVERWORLD_COUNT];
		String path = "Sprites/Buildings/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Castle/Castle");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Resource/Ore Smelters Camp");
		return sprites;
	}

    public int GetTileStart()
    {
        return TILE_START;
    }

    public int GetTotalTiles()
    {
        return TOTAL_TILE_COUNT;
    }

}
 
