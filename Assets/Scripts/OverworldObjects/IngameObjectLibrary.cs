﻿using System;
using UnityEngine;

/// <summary>
/// Ingame object library is a library containing all sprites used within the game. 
/// Some UI elements are left out of the library. This is a common system created to 
/// map sprite images to integers so that we can represent the whole map as a 2D matrix
/// of ints. 
/// 
/// The system is created for use with the map generator system.
/// </summary>
public class IngameObjectLibrary
{
			
    /// <summary>
    /// Konstruktør for InGameObjectLibrary som kjører kjører initialisering på alle sprites spille skal bruke
    /// </summary>
    public IngameObjectLibrary()
    {
		debug = InitializeDEBUGTiles();
		ground = InitializeTiles();
        environment = InitializeEnvironments();
		dwellings = InitializeDwellings();
        resourceBuildings = InitializeResouceBuildings();
        heroes = InitializeHeroes();
        castles = InitializeCastles();
        towns = InitializeTowns();
        ui = InitializeUI();
        portraits = InitializePortraits();
        units = InitializeUnits();
    }

    /// <summary>
    /// Kategorier for sprites
    /// </summary>
    public enum Category
    {
        Debug, Ground, Environment, Dwellings, ResourceBuildings, Heroes, Castle, Town, UI, Portraits, Resources, Unit, NOT_FOUND
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
            return RESOURCE_BUILDING_START;

        else if (category == Category.Dwellings)
            return DWELLINGS_START;

        else if (category == Category.Heroes)
            return HEROES_START;

        else if (category == Category.Castle)
            return CASTLES_START;

        else if (category == Category.Town)
            return TOWNS_START;

        else if (category == Category.UI)
            return UI_START;

        else if (category == Category.Portraits)
            return PORTRAIT_START;

        else if (category == Category.Unit)
            return UNIT_START;


        else if (category == Category.Debug)
            return DEBUG_SPRITES_START;

        else // fant ingenting
            return -1;
    }

    /// <summary>
    /// Returnerer kategori basert på global spriteID
    /// </summary>
    /// <param name="spriteID">Lokal spriteID</param>
    /// <returns>Enum category</returns>
    public static Category GetCategory(int spriteID)
    {
        if (spriteID < GROUND_START) // TODO: TA VEKK.
        {
            return Category.Debug;
        }
        // Antar at alle innverdier er større enn 2
        else if (spriteID < ENVIRONMENT_START && spriteID >= GROUND_START)
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
            return Category.Heroes;
        }
        else if (spriteID < TOWNS_START)
        {
            return Category.Castle;
        }
        else if (spriteID < UI_START)
        {
            return Category.Town;
        }
        else if (spriteID <= UI_START)
        {
            return Category.UI;
        }
        else if (spriteID <= PORTRAIT_START + PORTRAIT_COUNT)
        {
            return Category.Portraits;
        }

        // TODO: Debugger
        else Debug.Log("Error with spriteID= " + spriteID);
        return Category.NOT_FOUND;
    }

	Sprite[] debug;
	public const int DEBUG_SPRITES_START = 0;
	public const int DEBUG_SPRITES_COUNT = 4;

	/// <summary>
	/// Initialiserer et array for å holde på alle ground sprites
	/// </summary>
	/// <returns>Array med ground sprites</returns>
	private Sprite[] InitializeDEBUGTiles()
	{
		Sprite[] sprites = new Sprite[DEBUG_SPRITES_COUNT];
		String path = "Debug/";
		sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "CANNOTWALK");
		sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "CANWALK");
		sprites[2] = UnityEngine.Resources.Load<Sprite>(path + "TRIGGER");
        sprites[3] = UnityEngine.Resources.Load<Sprite>(path + "exitButton");

		return sprites;
	}

    // Ground-variabler. ground[] holder alle sprites, GROUND_START er global startverdi for ground sprites, GROUND_COUNT er antall ground sprites
    Sprite[] ground;
    public const int GROUND_START = DEBUG_SPRITES_START + DEBUG_SPRITES_COUNT;
    public const int GROUND_COUNT = 17;

    /// <summary>
    /// Initialiserer et array for å holde på alle ground sprites
    /// </summary>
    /// <returns>Array med ground sprites</returns>
    private Sprite[] InitializeTiles()
	{
		Sprite[] sprites = new Sprite[GROUND_COUNT];
		String path = "Sprites/Ground/";

	    sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Water/Water");

		// WATER-> GRASS TRANSITIONS:
		sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/north");
		sprites[2] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/east");
		sprites[3] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/south");
		sprites[4] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/west");
		sprites[5] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/northeast-in");
		sprites[6] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/southeast-in");
		sprites[7] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/southwest-in");
		sprites[8] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/northwest-in");
		sprites[9] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/northeast-out");
		sprites[10] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/southeast-out");
		sprites[11] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/southwest-out");
		sprites[12] = UnityEngine.Resources.Load<Sprite>(path + "Grass-Water/northwest-out");

	    sprites[13] = UnityEngine.Resources.Load<Sprite>(path + "Grass/grass1");
	    sprites[14] = UnityEngine.Resources.Load<Sprite>(path + "Grass/grass2");
	    sprites[15] = UnityEngine.Resources.Load<Sprite>(path + "Grass/grass3");
	    sprites[16] = UnityEngine.Resources.Load<Sprite>(path + "Grass/grass4");

		return sprites;
	}

    // Environment-variabler. environment[] holder alle sprites, ENVIRONMENT_START er global startverdi for environment sprites, ENVIRONMENT_COUNT er antall environment sprites
    Sprite[] environment;
    public const int ENVIRONMENT_START = GROUND_START + GROUND_COUNT;
    public const int ENVIRONMENT_COUNT = 7;

    /// <summary>
    /// Initialiserer et array for å holde på alle environment sprites
    /// </summary>
    /// <returns>Array med environment sprites</returns>
    private Sprite[] InitializeEnvironments()
    {
        Sprite[] sprites = new Sprite[ENVIRONMENT_COUNT];
        String path = "Sprites/Environment/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Forests/Forest 1");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Mountain1");
        sprites[2] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Mountain2");
        sprites[3] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Mountain3");
        sprites[4] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Mountain4");
        sprites[5] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Mountain5");
        sprites[6] = UnityEngine.Resources.Load<Sprite>(path + "Mountains/Below");

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
    public const int RESOURCE_BUILDING_COUNT = 4;

    /// <summary>
    /// Initialiserer et array for å holde på alle dwellings sprites
    /// </summary>
    /// <returns>Array med resourceBuildings sprites</returns>
    private Sprite[] InitializeResouceBuildings()
    {
        Sprite[] sprites = new Sprite[RESOURCE_BUILDING_COUNT];
        String path = "Sprites/Buildings/Resource/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Ore Smelters Camp");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Gold Mine Grass");
        sprites[2] = UnityEngine.Resources.Load<Sprite>(path + "Gem Mine Grass");
        sprites[3] = UnityEngine.Resources.Load<Sprite>(path + "Crystal Mine Grass");
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
        String path = "Sprites/Heroes/";

        // Overworld sprites
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "hero1");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "hero2");

        return sprites;
    }

    // castles-variabler. castles[] holder alle sprites, CASTLES_START er global startverdi for castles sprites, CASTLES_COUNT er antall castles sprites
    Sprite[] castles;
    public const int CASTLES_START = HEROES_START + HEROES_COUNT;
    public const int CASTLES_COUNT = 2;

    /// <summary>
    /// Initialiserer et array for å holde på alle castles sprites
    /// </summary>
    /// <returns>Array med castles sprites</returns>
    private Sprite[] InitializeCastles()
    {
        Sprite[] sprites = new Sprite[CASTLES_COUNT];
        String path = "Sprites/Buildings/Castle/";
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Castle");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "viking_castle");

        return sprites;
    }

    // towns-variabler. towns[] holder alle sprites, TOWNS_START er global startverdi for towns sprites, TOWNS_COUNT er antall towns sprites
    Sprite[] towns;
    public const int TOWNS_START = CASTLES_START + CASTLES_COUNT;
    public const int TOWNS_COUNT = 46;

    /// <summary>
    /// Initialiserer et array for å holde på alle towns sprites
    /// </summary>
    /// <returns>Array med towns sprites</returns>
    private Sprite[] InitializeTowns()
    {
        Sprite[] sprites = new Sprite[TOWNS_COUNT];
        String path = "Sprites/Buildings/Towns/";

        // Viking town sprites
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Background Viking Town");

        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Town Hall");
        sprites[2] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Pallisade");
        sprites[3] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Temple");
        sprites[4] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Dragon Tower");
        sprites[5] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Troll Cave");
        sprites[6] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Marketplace");
        sprites[7] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Beast Riders Guild");
        sprites[8] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Cernian Camp");
        sprites[9] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Training Camp");
        sprites[10] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Mage Tower");
        sprites[11] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Warriors Camp");
        sprites[12] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Workshop");

        // viking town can build blueprint sprites
        sprites[13] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Town Hall");
        sprites[14] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Pallisade");
        sprites[15] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Temple");
        sprites[16] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Dragon Tower");
        sprites[17] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Troll Cave");
        sprites[18] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Marketplace");
        sprites[19] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Beast Riders Guild");
        sprites[20] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Cernian Camp");
        sprites[21] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Training Camp");
        sprites[22] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Mage Tower");
        sprites[23] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Warriors Camp");
        // viking town has built sprites
        sprites[24] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Town Hall HasBought");
        sprites[25] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Pallisade HasBought");
        sprites[26] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Temple HasBought");
        sprites[27] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Dragon Tower HasBought");
        sprites[28] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Troll Cave HasBought");
        sprites[29] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Marketplace HasBought");
        sprites[30] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Beast Riders Guild HasBought");
        sprites[31] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Cernian Camp HasBought");
        sprites[32] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Training Camp HasBought");
        sprites[33] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Mage Tower HasBought");
        sprites[34] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Warriors Camp HasBought");
        // viking town can't build sprites
        sprites[35] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Town Hall NoBuy");
        sprites[36] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Pallisade NoBuy");
        sprites[37] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Temple NoBuy");
        sprites[39] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Dragon Tower NoBuy");
        sprites[39] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Troll Cave NoBuy");
        sprites[40] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Marketplace NoBuy");
        sprites[41] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Beast Riders Guild NoBuy");
        sprites[42] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Cernian Camp NoBuy");
        sprites[43] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Training Camp NoBuy");
        sprites[44] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Mage Tower NoBuy");
        sprites[45] = UnityEngine.Resources.Load<Sprite>(path + "Viking/Icons Warriors Camp NoBuy");


        return sprites;
    }

    // UI-variabler. UI[] holder alle sprites, UI_START er global startverdi for towns sprites, UI_COUNT er antall towns sprites
    Sprite[] ui;
    public const int UI_START = TOWNS_START + TOWNS_COUNT;
    public const int UI_COUNT = 11;

    /// <summary>
    /// Initialiserer et array for å holde på alle ui sprites
    /// </summary>
    /// <returns>Array med towns sprites</returns>
    private Sprite[] InitializeUI()
    {
        Sprite[] sprites = new Sprite[UI_COUNT];
        String path = "Sprites/UI/";

        // Viking town sprites
        // TODO: temp has duplicates to correspond to WINDOW TYPES
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "townhall_Card");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "tavern_card");
        sprites[2] = UnityEngine.Resources.Load<Sprite>(path + "tavern_card");
        sprites[3] = UnityEngine.Resources.Load<Sprite>(path + "tavern_card");
        sprites[4] = UnityEngine.Resources.Load<Sprite>(path + "unit_card");
        sprites[5] = UnityEngine.Resources.Load<Sprite>(path + "building_card");
        sprites[6] = UnityEngine.Resources.Load<Sprite>(path + "exit");
        sprites[7] = UnityEngine.Resources.Load<Sprite>(path + "buy");
        sprites[8] = UnityEngine.Resources.Load<Sprite>(path + "hero_frame");
        sprites[9] = UnityEngine.Resources.Load<Sprite>(path + "building_frame");
        sprites[10] = UnityEngine.Resources.Load<Sprite>(path + "resource_frame");
        return sprites;
    }

    // heroes-variabler. heroes[] holder alle sprites, HEROES_START er global startverdi for heroes sprites, HEROES_COUNT er antall heroes sprites
    Sprite[] portraits;
    public const int PORTRAIT_START = UI_START + UI_COUNT;
    public const int PORTRAIT_COUNT = 5;

    /// <summary>
    /// Initialiserer et array for å holde på alle heroes sprites
    /// </summary>
    /// <returns>Array med heroes sprites</returns>
    private Sprite[] InitializePortraits()
    {
        Sprite[] sprites = new Sprite[PORTRAIT_COUNT];
        String path = "Sprites/Heroes/";

        // portrait sprites
        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "BlueBerry");
        sprites[1] = UnityEngine.Resources.Load<Sprite>(path + "Gork");
        sprites[2] = UnityEngine.Resources.Load<Sprite>(path + "Jack McBlackwell");
        sprites[3] = UnityEngine.Resources.Load<Sprite>(path + "Johny Mudbone");
        sprites[4] = UnityEngine.Resources.Load<Sprite>(path + "Mantooth");

        return sprites;
    }

    // resources-variabler. resources[] holder alle sprites, resources_START er global startverdi for heroes sprites, resources_COUNT er antall resources sprites
    Sprite[] units;
    public const int UNIT_START = UI_START + UI_COUNT;
    public const int UNIT_COUNT = 0;

    /// <summary>
    /// Initialiserer et array for å holde på alle resources sprites
    /// </summary>
    /// <returns>Array med resources sprites</returns>
    private Sprite[] InitializeUnits()
    {
        Sprite[] sprites = new Sprite[PORTRAIT_COUNT];
        String path = "Sprites/Units/";

        sprites[0] = UnityEngine.Resources.Load<Sprite>(path + "stone_troll_portrait");

        return sprites;
    }

    /// <summary>
    /// Gjør global spriteID til lokal spriteID for ground tiles
    /// </summary>
    /// <param name="spriteID">Global spriteID</param>
    /// <returns>Lokal spriteID</returns>
    public Sprite GetGround(int spriteID)
    {
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

    public Sprite GetHero(int spriteID)
    {
        return heroes[spriteID - HEROES_START];
    }

    public Sprite GetTown(int spriteID)
    {
        return towns[spriteID - TOWNS_START];
    }

    public Sprite GetUI(int spriteID)
    {
        return ui[spriteID - UI_START];
    }

    public Sprite GetPortrait(int spriteID)
    {
        return portraits[spriteID - PORTRAIT_START];
    }

    public Sprite GetUnit(int spriteID)
    {
        return units[spriteID - UNIT_START];
    }

    public Sprite GetDebugSprite(int spriteID)
	{
		return debug[spriteID];
	}

    public static Sprite[] ResourceIcons = UnityEngine.Resources.LoadAll<Sprite>("Sprites/UI/Resources");
    public static Sprite[] Resource = UnityEngine.Resources.LoadAll<Sprite>("Sprites/Environment/Resources");

    public static Sprite[] Pointers = UnityEngine.Resources.LoadAll<Sprite>("Sprites/Pointers");

    private static string unitPath = "Sprites/Units";
    public static Sprite[] unitSprite = UnityEngine.Resources.LoadAll<Sprite>(unitPath + "Sprite");
    public static Sprite[] unitArt = UnityEngine.Resources.LoadAll<Sprite>(unitPath + "Art");
    public static Sprite[] unitPortraits = UnityEngine.Resources.LoadAll<Sprite>(unitPath + "Portraits");
    
    private static string heroPath = "Sprites/Hero";
    public static Sprite[] heroSprite = UnityEngine.Resources.LoadAll<Sprite>(unitPath + "Sprite");
    public static Sprite[] heroPortraits = UnityEngine.Resources.LoadAll<Sprite>(unitPath + "Portraits");

    private static string townPath = "Sprites/Town";
    public static Sprite[] townOverworld = UnityEngine.Resources.LoadAll<Sprite>(townPath + "Sprite");
    public static Sprite[] townScreen = UnityEngine.Resources.LoadAll<Sprite>(townPath + "Portraits");
}
 
