using System;
using UnityEngine;

public class IngameObjectLibrary
{
			
    // Initialiserer alle sprites når et objekt blir laget
    public IngameObjectLibrary()
    {
		ground = InitializeTiles();
        environment = InitializeEnvironments();
		dwellings = InitializeDwellings();
        resourceBuildings = InitializeResouceBuildings();
        heroes = InitializeHeroes();
        castles = InitializeCastles();
	}

    public enum Category
    {
        Ground, Environment, Dwellings, ResourceBuildings, Heroes, Castle, NOT_FOUND
    }

    public static int GetOffset( Category category )
    {
        if (category == Category.Ground)
            return GROUND_START;

        else if (category == Category.Environment)
            return ENVIRONMENT_START;

        else if (category == Category.ResourceBuildings)
            return DWELLINGS_START;

        else if (category == Category.Dwellings)
            return DWELLINGS_START;

        else if (category == Category.Heroes)
            return HEROES_START;

        else // CASTLE
            return CASTLES_START;
    }

    public Category GetCategory(int spriteID)
    {
        // Antar at alle innverdier er større enn 2
        if (spriteID < ENVIRONMENT_START)
        {
            return Category.Ground;
        }
        else if (spriteID < DWELLINGS_START)
        {
            return Category.Environment;
        }
        else if (spriteID < RESOURCE_BUILDING_START)
        {
            return Category.Dwellings;
        }
        else if (spriteID < HEROES_START)
        {
            return Category.ResourceBuildings;
        }
        else if (spriteID <= CASTLES_START + CASTLES_COUNT)
        {
            return Category.Castle;
        }
        // TODO: Debugger
        else Debug.Log("Error with spriteID= " + spriteID);
        return Category.NOT_FOUND;
    }

    // ground
    Sprite[] ground;
    public const int GROUND_START = 3;
    const int GROUND_COUNT = 2;

    // Initialiserer alles tile Sprites, nye legges inn manuelt
    private Sprite[] InitializeTiles()
	{
		Sprite[] sprites = new Sprite[GROUND_COUNT];
		String path = "Sprites/Ground/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Grass/Grass");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Water/Water"); 

        return sprites;
	}

    // environments (skog, mountains, etc)
    Sprite[] environment;
    public const int ENVIRONMENT_START = GROUND_START + GROUND_COUNT;
    public const int ENVIRONMENT_COUNT = 1;

    private Sprite[] InitializeEnvironments()
    {
        Sprite[] sprites = new Sprite[ENVIRONMENT_COUNT];
        String path = "Sprites/Environment/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Placeholder/Forest");

        return sprites;
    }

    // dwellings
    Sprite[] dwellings;
    const int DWELLINGS_START = ENVIRONMENT_START + ENVIRONMENT_COUNT;
    const int DWELLINGS_COUNT = 1;

    // Initialiserer alles building overworld Sprites, nye legges inn manuelt
    private Sprite[] InitializeDwellings()
    {
        Sprite[] sprites = new Sprite[DWELLINGS_COUNT];
        String path = "Sprites/Buildings/Dwelling/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "tempDwelling");
        return sprites;
    }

    //resource buildings
    Sprite[] resourceBuildings;
    const int RESOURCE_BUILDING_START = DWELLINGS_START + DWELLINGS_COUNT;
    const int RESOURCE_BUILDING_COUNT = 1;

    private Sprite[] InitializeResouceBuildings()
    {
        Sprite[] sprites = new Sprite[RESOURCE_BUILDING_COUNT];
        String path = "Sprites/Buildings/Resource/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Ore Smelters Camp");
        return sprites;
    }

    // heroes
    Sprite[] heroes;
    const int HEROES_START = RESOURCE_BUILDING_START + RESOURCE_BUILDING_COUNT;
    const int HEROES_COUNT = 2;

    private Sprite[] InitializeHeroes()
    {
        Sprite[] sprites = new Sprite[HEROES_COUNT];
        String path = "Sprites/Heroes";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "hero1");
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "hero2");

        return sprites;
    }

    // castles
    Sprite[] castles;
    const int CASTLES_START = HEROES_START + HEROES_COUNT;
    const int CASTLES_COUNT = 1;

    private Sprite[] InitializeCastles()
    {
        Sprite[] sprites = new Sprite[CASTLES_COUNT];
        String path = "Sprites/Buildings/Castle/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Castle");

        return sprites;
    }

    // Regner fra global spriteID til lokal spriteID
    public Sprite GetGround(int spriteID)
    {
        //Debug.Log("id=" + spriteID+", local=" + (spriteID - GROUND_START));
        return ground[spriteID - GROUND_START];
    }

    // Regner fra global spriteID til lokal spriteID
    public Sprite GetEnvironment(int spriteID)
	{
		return environment[spriteID - ENVIRONMENT_START];
	}

    public Sprite GetDwelling(int spriteID)
    {
        return dwellings[spriteID - DWELLINGS_START];
    }

    public Sprite GetResourceBuilding(int spriteID)
    {
        return resourceBuildings[spriteID - RESOURCE_BUILDING_START];
    }

    public Sprite GetCastle(int spriteID)
    {
        return castles[spriteID - CASTLES_START];
    }
}
 
