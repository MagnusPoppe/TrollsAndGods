using System;
using UnityEngine;

public class IngameObjectLibrary
{
			
    /// <summary>
    /// Konstruktør for InGameObjectLibrary som kjører kjører initialisering på alle sprites spille skal bruke
    /// </summary>
    public IngameObjectLibrary()
    {
		ground = InitializeTiles();
        environment = InitializeEnvironments();
		dwellings = InitializeDwellings();
        resourceBuildings = InitializeResouceBuildings();
        heroes = InitializeHeroes();
        castles = InitializeCastles();
        towns = InitializeTowns();
	}

    /// <summary>
    /// Kategorier for sprites
    /// </summary>
    public enum Category
    {
        Ground, Environment, Dwellings, ResourceBuildings, Heroes, Castle, Town, NOT_FOUND
    }

    /// <summary>
    /// Regner ut global spriteID basert på lokal spriteID og kategori
    /// </summary>
    /// <param name="category">Enum kategori</param>
    /// <returns>Returnerer startverdien for en kategori av sprites</returns>
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

        else if (category == Category.Castle) // CASTLE
            return CASTLES_START;

        else if (category == Category.Town)
            return TOWNS_START;

        else // fant ingenting
            return -1;
    }

    /// <summary>
    /// Returnerer kategori basert på global spriteID
    /// </summary>
    /// <param name="spriteID">Lokal spriteID</param>
    /// <returns>Enum category</returns>
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
        else if (spriteID < CASTLES_START)
        {
            return Category.Castle;
        }
        else if (spriteID <= TOWNS_START + TOWNS_COUNT)
            return Category.Town;

        // TODO: Debugger
        else Debug.Log("Error with spriteID= " + spriteID);
        return Category.NOT_FOUND;
    }

    // Ground-variabler. ground[] holder alle sprites, GROUND_START er global startverdi for ground sprites, GROUND_COUNT er antall ground sprites
    Sprite[] ground;
    public const int GROUND_START = 3;
    public const int GROUND_COUNT = 2;

    /// <summary>
    /// Initialiserer et array for å holde på alle ground sprites
    /// </summary>
    /// <returns>Array med ground sprites</returns>
    private Sprite[] InitializeTiles()
	{
		Sprite[] sprites = new Sprite[GROUND_COUNT];
		String path = "Sprites/Ground/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Grass/Grass");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Water/Water"); 

        return sprites;
	}

    // Environment-variabler. environment[] holder alle sprites, ENVIRONMENT_START er global startverdi for environment sprites, ENVIRONMENT_COUNT er antall environment sprites
    Sprite[] environment;
    public const int ENVIRONMENT_START = GROUND_START + GROUND_COUNT;
    public const int ENVIRONMENT_COUNT = 6;

    /// <summary>
    /// Initialiserer et array for å holde på alle environment sprites
    /// </summary>
    /// <returns>Array med environment sprites</returns>
    private Sprite[] InitializeEnvironments()
    {
        Sprite[] sprites = new Sprite[ENVIRONMENT_COUNT];
        String path = "Sprites/Environment/";
        //sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Placeholder/Forest");
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Forests/Forest 1");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Mountain 1");
        sprites[2] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Mountain 2");
        sprites[3] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Mountain 3");
        sprites[4] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Mountain 4");
        sprites[5] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Mountain 5");

        return sprites;
    }

    // dwellings-variabler. dwellings[] holder alle sprites, DWELLINGS_START er global startverdi for dwellings sprites, DWELLINGS_COUNT er antall dwellings sprites
    Sprite[] dwellings;
    public const int DWELLINGS_START = ENVIRONMENT_START + ENVIRONMENT_COUNT;
    public const int DWELLINGS_COUNT = 1;

    /// <summary>
    /// Initialiserer et array for å holde på alle dwellings sprites
    /// </summary>
    /// <returns>Array med dwellings sprites</returns>
    private Sprite[] InitializeDwellings()
    {
        Sprite[] sprites = new Sprite[DWELLINGS_COUNT];
        String path = "Sprites/Buildings/Dwelling/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "tempDwelling");
        return sprites;
    }

    // resourceBuildings-variabler. resourceBuildings[] holder alle sprites, RESOURCE_BUILDING_START er global startverdi for resourceBuildings sprites, RESOURCE_BUILDING_COUNT er antall resourceBuildings sprites
    Sprite[] resourceBuildings;
    public const int RESOURCE_BUILDING_START = DWELLINGS_START + DWELLINGS_COUNT;
    public const int RESOURCE_BUILDING_COUNT = 1;

    /// <summary>
    /// Initialiserer et array for å holde på alle dwellings sprites
    /// </summary>
    /// <returns>Array med resourceBuildings sprites</returns>
    private Sprite[] InitializeResouceBuildings()
    {
        Sprite[] sprites = new Sprite[RESOURCE_BUILDING_COUNT];
        String path = "Sprites/Buildings/Resource/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Ore Smelters Camp");
        return sprites;
    }

    // heroes-variabler. heroes[] holder alle sprites, HEROES_START er global startverdi for heroes sprites, HEROES_COUNT er antall heroes sprites
    Sprite[] heroes;
    public const int HEROES_START = RESOURCE_BUILDING_START + RESOURCE_BUILDING_COUNT;
    public const int HEROES_COUNT = 2;

    /// <summary>
    /// Initialiserer et array for å holde på alle heroes sprites
    /// </summary>
    /// <returns>Array med heroes sprites</returns>
    private Sprite[] InitializeHeroes()
    {
        Sprite[] sprites = new Sprite[HEROES_COUNT];
        String path = "Sprites/Heroes";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "hero1");
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "hero2");

        return sprites;
    }

    // castles-variabler. castles[] holder alle sprites, CASTLES_START er global startverdi for castles sprites, CASTLES_COUNT er antall castles sprites
    Sprite[] castles;
    public const int CASTLES_START = HEROES_START + HEROES_COUNT;
    public const int CASTLES_COUNT = 1;

    /// <summary>
    /// Initialiserer et array for å holde på alle castles sprites
    /// </summary>
    /// <returns>Array med castles sprites</returns>
    private Sprite[] InitializeCastles()
    {
        Sprite[] sprites = new Sprite[CASTLES_COUNT];
        String path = "Sprites/Buildings/Castle/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Castle");

        return sprites;
    }

    // towns-variabler. towns[] holder alle sprites, TOWNS_START er global startverdi for towns sprites, TOWNS_COUNT er antall towns sprites
    Sprite[] towns;
    public const int TOWNS_START = CASTLES_START + CASTLES_COUNT;
    public const int TOWNS_COUNT = 2;

    /// <summary>
    /// Initialiserer et array for å holde på alle towns sprites
    /// </summary>
    /// <returns>Array med towns sprites</returns>
    private Sprite[] InitializeTowns()
    {
        Sprite[] sprites = new Sprite[HEROES_COUNT];
        String path = "Sprites/Buildings/Towns/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "castle_town/castle_town_without_tower");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "castle_town/castle_town_tower");

        return sprites;
    }

    /// <summary>
    /// Gjør global spriteID til lokal spriteID for ground tiles
    /// </summary>
    /// <param name="spriteID">Global spriteID</param>
    /// <returns>Lokal spriteID</returns>
    public Sprite GetGround(int spriteID)
    {
        //Debug.Log("id=" + spriteID+", local=" + (spriteID - GROUND_START));
        return ground[spriteID - GROUND_START];
    }

    /// <summary>
    /// Gjør global spriteID til lokal spriteID for environment tiles
    /// </summary>
    /// <param name="spriteID">Global spriteID</param>
    /// <returns>Lokal spriteID</returns>
    public Sprite GetEnvironment(int spriteID)
	{
		return environment[spriteID - ENVIRONMENT_START];
	}

    /// <summary>
    /// Gjør global spriteID til lokal spriteID for dwelling tiles
    /// </summary>
    /// <param name="spriteID">Global spriteID</param>
    /// <returns>Lokal spriteID</returns>
    public Sprite GetDwelling(int spriteID)
    {
        return dwellings[spriteID - DWELLINGS_START];
    }

    /// <summary>
    /// Gjør global spriteID til lokal spriteID for resource building tiles
    /// </summary>
    /// <param name="spriteID">Global spriteID</param>
    /// <returns>Lokal spriteID</returns>
    public Sprite GetResourceBuilding(int spriteID)
    {
        return resourceBuildings[spriteID - RESOURCE_BUILDING_START];
    }

    /// <summary>
    /// Gjør global spriteID til lokal spriteID for castle tiles
    /// </summary>
    /// <param name="spriteID">Global spriteID</param>
    /// <returns>Lokal spriteID</returns>
    public Sprite GetCastle(int spriteID)
    {
        return castles[spriteID - CASTLES_START];
    }

    /// <summary>
    /// Gjør global spriteID til lokal spriteID for town tiles
    /// </summary>
    /// <param name="spriteID">Global spriteID</param>
    /// <returns>Lokal spriteID</returns>
    public Sprite GetTown(int spriteID)
    {
        return towns[spriteID - TOWNS_START];
    }
}
 
