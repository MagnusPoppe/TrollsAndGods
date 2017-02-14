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
	const int TILE_START = 0;
	const int TOTAL_TILE_COUNT = 2;

	Sprite[] buildings_overworld;
	const int BUILDING_OVERWORLD_START = TILE_START + TOTAL_TILE_COUNT;
	const int TOTAL_BUILDING_OVERWORLD_COUNT = 1;


	Sprite[] buildings_town;
				

    public IngameObjectLibrary()
    {
		tiles = InitializeTiles();
		buildings_overworld = InitializeBuildings();
	}

	public Sprite GetTile(int spriteID)
	{
		return tiles[spriteID];
	}

	private Sprite[] InitializeTiles()
	{
		Sprite[] sprites = new Sprite[TOTAL_TILE_COUNT];
		String path = "Sprites/Tiles/";
		sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Grass/Grass");
		sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Water/Water");
		return sprites;
	}

	public Sprite GetBuilding(int spriteID)
	{
		return buildings_overworld[spriteID];
	}

	private Sprite[] InitializeBuildings()
	{
		Sprite[] sprites = new Sprite[TOTAL_BUILDING_OVERWORLD_COUNT];
		String path = "Sprites/Buildings/";
		sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Resource/Ore Smelters Camp");
		return sprites;
	}
}
 
