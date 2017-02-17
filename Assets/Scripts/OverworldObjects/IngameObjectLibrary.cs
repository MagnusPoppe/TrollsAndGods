using System;
using UnityEngine;

public class IngameObjectLibrary
{
 //   CONSTANTS:
 //   const int ENVIROMENT_TYPES = 8;
 //   const int OBJECT_TYPES     = 50;

	//OBJECT TYPES (ARRAY 1-DIMENTION):
	//const int TILE_TYPE = 0;
	//const int BUILDING_TYPE_RESOURCE = 1;
	//const int BUILDING_TYPE_DWELLING = 2;
	//const int BUILDING_TYPE_TOWN = 3;
	//const int BUILDING_TYPE_MISC = 4;
	//const int PICKUP_TYPE_RESOURCE = 5;
	//const int PICKUP_TYPE_ARTIFACT = 6;

 //   TILETYPES (ARRAY 2-DIMENTION):
 //   const int ENVIROMENT_NEUTRAL        = 0;
 //   const int ENVIROMENT_DIRT           = 1;
 //   const int ENVIROMENT_GRASS          = 2;
 //   const int ENVIROMENT_LAVA           = 3;
 //   const int ENVIROMENT_WATER          = 4;
 //   const int ENVIROMENT_DARKNESS       = 5;

 //   TILE TYPES (ARRAY 3-DIMENTION):
 //   const int NORTH_EDGE     = 0;
 //   const int NORTHEAST_EDGE = 1;
 //   const int EAST_EDGE      = 2;
 //   const int SOUTHEAST_EDGE = 3;
 //   const int SOUTH_EDGE     = 4;
 //   const int SOUTHWEST_EDGE = 5;
 //   const int WEST_EDGE      = 6;
 //   const int NORTHWEST_EDGE = 7;


    // ground
	Sprite[] ground;					
	const int GROUND_START = 3;
	const int TOTAL_GROUND_COUNT = 3;

    // overworld buildings TODO: gjør mindre generell
    Sprite[] buildings_overworld;
    const int BUILDING_OVERWORLD_START = GROUND_START + TOTAL_GROUND_COUNT;
	const int TOTAL_BUILDING_OVERWORLD_COUNT = 1;

    // heroes
    Sprite[] heroes;
    const int HEROES_START = BUILDING_OVERWORLD_START + TOTAL_BUILDING_OVERWORLD_COUNT;
    const int TOTAL_HEROES_COUNT = 1;

    // castles
    Sprite[] castles;
    const int CASTLES_START = HEROES_START + TOTAL_HEROES_COUNT;
    const int TOTAL_CASTLES_COUNT = 1;


    Sprite[] buildings_town;
				
    // Initialiserer alle sprites når et objekt blir laget
    public IngameObjectLibrary()
    {
		ground = InitializeTiles();
		buildings_overworld = InitializeBuildings();
        heroes = InitializeHeroes();
	}

    public enum Category
    {
        Ground, ResourceBuilding, Dwelling, Heroes, Castle
    }

    public static int GetOffset( Category category )
    {
        if (category == Category.Ground)
            return GROUND_START;

        else if (category == Category.ResourceBuilding)
            return BUILDING_OVERWORLD_START;

        else if (category == Category.Dwelling)
            return BUILDING_OVERWORLD_START;

        else if (category == Category.Heroes)
            return HEROES_START;

        else // CASTLE
            return BUILDING_OVERWORLD_START;
    }

    public Category GetCategory(int spriteID)
    {
        if (spriteID < BUILDING_OVERWORLD_START)
        {
            return Category.Ground;
        }
        else if (spriteID < HEROES_START)
        {
            return Category.ResourceBuilding;
        }
        else return Category.Castle;
    }

    // Initialiserer alles tile Sprites, nye legges inn manuelt
	private Sprite[] InitializeTiles()
	{
		Sprite[] sprites = new Sprite[TOTAL_GROUND_COUNT];
		String path = "Sprites/Ground/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Water/Water");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Grass/Grass");
		sprites[2] = UnityEngine.Resources.Load<Sprite>(path + "Placeholder/Forest");

        return sprites;
	}

    // Initialiserer alles building overworld Sprites, nye legges inn manuelt
    private Sprite[] InitializeBuildings()
    {
        Sprite[] sprites = new Sprite[TOTAL_BUILDING_OVERWORLD_COUNT];
        String path = "Sprites/Buildings/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Resource/Ore Smelters Camp");
        return sprites;
    }

    private Sprite[] InitializeHeroes()
    {
        Sprite[] sprites = new Sprite[TOTAL_HEROES_COUNT];
        String path = "Sprites/Heroes";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "hero1");
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "hero2");

        return sprites;
    } 

    private Sprite[] InitializeCastles()
    {
        Sprite[] sprites = new Sprite[TOTAL_CASTLES_COUNT];
        String path = "Sprites/Buildings/Castle/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Castle");

        return sprites;
    }

    // Regner fra global spriteID til lokal spriteID
    public Sprite GetGround(int spriteID)
    {
        return ground[spriteID - GROUND_START];
    }

    // Regner fra global spriteID til lokal spriteID
    public Sprite GetBuilding(int spriteID)
	{
		return buildings_overworld[spriteID-BUILDING_OVERWORLD_START];
	}

    public Sprite GetCastle(int spriteID)
    {
        return castles[spriteID - CASTLES_START];
    }
}
 
