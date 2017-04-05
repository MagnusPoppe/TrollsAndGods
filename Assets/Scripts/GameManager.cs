using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using TownView;
using MapGenerator;
using OverworldObjects;
using UI;
using Units;

public class GameManager : MonoBehaviour
{

    public MapMaker mapmaker;

    // Loads in camera variables
    Camera mainCamera;

    public CameraMovement cameraMovement;

	// ONLY SET FOR USE WITH UNITY EDITOR!
	public bool CanWalkDebugMode = false;
    public bool DrawCostalTilesTransitions = true;

    public int WIDTH = 64;
    public int HEIGHT = 64;
    [Range(0, 20)]
    int buildingCount;
    // VORONOI varables:
    [Range(0, 50)]
    public int sites = 8;
    [Range(1, 20)]
    public int relaxIterations = 3;
    [Range(0, 20)]
    public int smoothIterations = 5;
    public string seed = "Angelica";
    [Range(0, 100)]
    public int fillpercentWalkable = 57;

    // Map Globals:
    int width, height;
    static public IngameObjectLibrary libs;
    AStarAlgo aStar;
    GameObject[,] tiles;
    public const float XRESOLUTION = 2598;
    public const float YRESOLUTION = 1299;
    public const float YOFFSET = YRESOLUTION / XRESOLUTION;

    // Generated from mapmaker class:
    Region[] regions;
    int[,] canWalk;
    Placement placement;
    Reaction[,] reactions;

    // Graphical elements
    GameObject[,] groundLayer;
    GameObject[,] buildingLayer;
    public GameObject[,] heroLayer;

    // GameManager
    int amountOfPlayers;
    Player[] players;
    public int WhoseTurn { get; private set; }
    Date date;

    // Click listeners
    const int CLICKSPEED = 35;
    bool prepareDoubleClick;
    int clickCount;
    public Vector2 savedClickedPos;

    public Hero activeHero;
    public Hero[] heroes = new Hero[5];

    // Town
    GameObject[] buildingsInActiveTown;
    GameObject[] armyInActiveTown;
    GameObject townArmyCanvas;
    GameObject townWindow;
    public GameObject swapObject;
    bool overWorld;
    public GameObject townArmyPanel;
    public GameObject townHallPanel;
    public GameObject tavernPanel;
    public GameObject marketplacePanel;
    public GameObject dwellingPanel;
    public GameObject buildingPanel;
    public GameObject townToDestroyPanel;

    // UI
    Button nextRoundBtn;
    Text dateText;
    Text[] resourceText;
    string[] resourceTextPosition = new string[] { "TextGold", "TextWood", "TextOre", "TextCrystal", "TextGem" };
    GameObject overWorldCanvas;
    GameObject overworldInteractablePanel;
    Vector2 overworldInteractablePanelPosition;
    GameObject[] heroObjects;
    GameObject[] townObjects;
    Button[] heroButton;
    Button[] townButton;
    public GameObject adjustResourcePanel;
    public GameObject purchaseButton;
    public GameObject unitPanel;

    //currentReaction
    private Reaction curReaction;

    //Combat
    private GraphicalBattlefield graphicalBattlefield;
    private GameObject combatWindow;

    //Movement
    private MovementManager movement;
    public GameObject parentToMarkers;
    public bool heroActive;
    public GameObject activeHeroObject;

    // Use this for initialization
    void Start ()
    {
        heroes[0] = new Blueberry();
        heroes[1] = new Gork();
        heroes[2] = new JackMcBlackwell();
        heroes[3] = new JohnyMudbone();
        heroes[4] = new Mantooth();

        heroObjects = new GameObject[8];
        townObjects = new GameObject[10];
        heroButton = new Button[8];
        townButton = new Button[10];

        adjustResourcePanel = GameObject.Find("OverworldAdjustResourcePanel");
        adjustResourcePanel.SetActive(false);
        purchaseButton = adjustResourcePanel.transform.GetChild(1).gameObject;
        // Initialize TownPanels and deactivate them
        townToDestroyPanel = GameObject.Find("TownToDestroyPanel");
        townArmyPanel = GameObject.Find("TownArmyPanel");
        townArmyPanel.SetActive(false);
        townHallPanel = GameObject.Find("TownHallPanel");
        townHallPanel.SetActive(false);
        tavernPanel = GameObject.Find("TavernPanel");
        tavernPanel.SetActive(false);
        marketplacePanel = GameObject.Find("MarketplacePanel");
        marketplacePanel.SetActive(false);
        dwellingPanel = GameObject.Find("DwellingPanel");
        dwellingPanel.SetActive(false);
        buildingPanel = GameObject.Find("BuildingPanel");
        buildingPanel.SetActive(false);
        unitPanel = GameObject.Find("UnitPanel");
        unitPanel.SetActive(false);


        parentToMarkers = new GameObject();
        parentToMarkers.name = "Path";
        width = WIDTH;
        height = HEIGHT;
        // Initialize sprite library
        libs = new IngameObjectLibrary();

        // CREATING THE MAP USING MAPMAKER
        amountOfPlayers = 3;
        players = new Player[amountOfPlayers];
        reactions = new Reaction[width, height];
        GenerateMap();

        // Creating the camera game object and variables
        GameObject tempCameraObject = GameObject.Find("Main Camera");
        mainCamera = tempCameraObject.GetComponent<Camera>();
        cameraMovement = tempCameraObject.GetComponent<CameraMovement>();

        // Set active Hero
        heroActive = true;
        activeHero = players[0].Heroes[0];
        activeHeroObject = heroLayer[activeHero.Position.x, activeHero.Position.y];

        // Initialize turn based variables and date
        WhoseTurn = 0;
        clickCount = 0;
        date = new Date();

        //savedClickedPos = HandyMethods.getIsoTilePos(transform.position);
        aStar = new AStarAlgo(canWalk, width, height, false);
        townWindow = GameObject.Find("Town");
        townWindow.SetActive(false);
        overWorld = true;
        GenerateUI();

        //fetch Combat references
        combatWindow = GameObject.Find("Combat");
        combatWindow.SetActive(false);
        graphicalBattlefield = combatWindow.GetComponent<GraphicalBattlefield>();

        // Starting movementmanager:
        movement = new MovementManager(players, reactions, canWalk, aStar, this);
        movement.activeHero = activeHero;
    }

	// Update is called once per frame
	void Update ()
    {
        if (overWorld)
        {
            ListenToInputs();

            // Upon every update, activedhero will be moved in a direction if walking is enabled
            if (movement.walking)
            {
                movement.Walk();
            }
            //Nothing is clicked and hero is not walking, listener for change mouse hover
            else
            {
                ListenToMouseHover();
            }
        }
    }

    public void ListenToInputs()
    {
        // if you have clicked once on a castle of possession, give a window of frames to click it again to open castle menu
        if (prepareDoubleClick && ++clickCount == CLICKSPEED)
        {
            clickCount = 0;
            prepareDoubleClick = false;
        }
        // Left click listener
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Fetch the point just clicked and adjust the position in the square to the corresponding isometric position
            Vector2 posClicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posClicked = HandyMethods.getIsoTilePos(posClicked).ToVector2();

            int x = (int)posClicked.x;
            int y = (int)posClicked.y;
            // Owners castle is clicked
            if (canWalk[(int)posClicked.x, (int)posClicked.y] != MapMaker.TRIGGER && reactions[x, y] != null && reactions[x, y].GetType().Equals(typeof(CastleReact)))
            {
                if (prepareDoubleClick)
                {
                    CastleReact castleClicked = (CastleReact)reactions[x, y];
                    if (players[WhoseTurn].equals(castleClicked.Castle.Player))
                    {
                        castleClicked.React(players[WhoseTurn]);
                        Debug.Log("Leftclicked your own castle");
                    }
                }
                else
                    prepareDoubleClick = true;
            }
            // Hero is active, either try to make a path to pointed destination, or activate walking towards there.
            else if (heroActive && movement.activeHero.Player.equals(players[WhoseTurn]))
            {
                if (movement.walking)
                {
                    movement.lastStep = true;
                }
                // Hero's own position is clicked
                else if (movement.activeHero.Position.Equals(new Point(posClicked)))
                {
                    Debug.Log("Clicked on activated hero");
                    // Todo, open hero menu
                }
                // If an open square is clicked
                else if (canWalk[(int)posClicked.x, (int)posClicked.y] != MapMaker.CANNOTWALK)
                {
                    // Walk to pointer if marked square is clicked by enabling variables that triggers moveHero method on update
                    if (movement.pathMarked && posClicked.Equals(savedClickedPos) && movement.activeHero.CurMovementSpeed > 0)
                    {
                        movement.walking = true;
                        movement.destination = new Point(movement.activeHero.Path[movement.tilesWalking - 1]);
                    }
                    // Activate clicked path
                    else
                    {
                        movement.pathObjects = movement.MarkPath(posClicked);
                    }
                }
            }
            // activate hero that you clicked on (check after pathing test, to also allow you to walk to that hero)
            else if (reactions[x, y] != null && reactions[x, y].GetType().Name.Equals(typeof(HeroMeetReact)))
            {
                HeroMeetReact heroClicked = (HeroMeetReact)reactions[x, y];
                if (players[WhoseTurn].equals(heroClicked.Hero.Player))
                {
                    // TODO when click on your own hero
                    Debug.Log("Leftclicked your own hero");
                    heroActive = true;
                    activeHero = heroClicked.Hero;
                    movement.activeHero = activeHero;
                    cameraMovement.centerCamera(activeHero.Position.ToVector2());
                }
            }
        }

        // Center camera around first hero or castle
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (players[WhoseTurn].Heroes[0] != null)
            {
                cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(activeHero.Position));
            }
            else
            {
                cameraMovement.centerCamera(players[WhoseTurn].Castle[0].GetPosition().ToVector2());
            }
        }
        // Nextturn by enter
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            nextTurn();
        }
        //rightclick for combat for testing
        else if (Input.GetMouseButtonDown(1))
        {
            if (activeHero.Units.GetUnits()[0] == null)
            {
                activeHero.Units.setUnit(new StoneTroll(), 5, 0);
            }
            UnitTree defendingTest = new UnitTree();
            defendingTest.setUnit(new StoneTroll(), 5, 0);
            enterCombat(15, 11, activeHero, defendingTest);
        }
    }

    public void ListenToMouseHover()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = HandyMethods.getIsoTilePos(mousePos).ToVector2();
        int x = (int)mousePos.x;
        int y = (int)mousePos.y;
        if (x >= 0 && x < width && y >= 0 && y < height && reactions[x, y] != null)
        {
            if (reactions[x, y].GetType().Equals(typeof(CastleReact)))
            {
                CastleReact cr = (CastleReact)reactions[x, y];
                if (players[WhoseTurn].equals(cr.Castle.Player))
                {
                    // TODO when you hover over your own castle, change mouse pointer
                }
            }
            else if (reactions[x, y].GetType().Equals(typeof(HeroMeetReact)))
            {
                HeroMeetReact hmr = (HeroMeetReact)reactions[x, y];
                if (players[WhoseTurn].equals(hmr.Hero.Player))
                {
                    // TODO when you hover over an hero, change mouse pointer
                }
            }
            else if (reactions[x, y].GetType().Equals(typeof(UnitReaction)))
            {
                // TODO when you hover over an neutral unit, change mouse pointer
            }
        }
    }

	/// <summary>
	/// Generates the map. This replaces the "map.cs" file.
	/// </summary>
	private void GenerateMap()
	{


		mapmaker = new MapMaker(
            players, width, height, 40,                     // Map Properites TODO: fjern parameter 40/length 
			seed, fillpercentWalkable, smoothIterations,    // BinaryMap Properities
			sites, relaxIterations,                         // Voronoi Properties
			buildingCount,
            DrawCostalTilesTransitions
		);

        int[,] map = mapmaker.GetMap();

		// SETTING GLOBALS:
		regions = mapmaker.GetRegions();
		canWalk = mapmaker.GetCanWalkMap();

        // SETTING UP REGIONS WITH PLAYERS, CASTLE AND HERO:
        mapmaker.initializePlayers(map, canWalk, players);

	    // Placeing all buildings within the regions.
	    placement = mapmaker.PlaceBuildings(players);
        PlaceBuildings();
        
        // Add reactions to buildings in regions
        foreach (Region r in regions)
        {
            if (r.GetType().Equals(typeof(LandRegion)))
            {
                LandRegion lr = (LandRegion)r;
                lr.makeReactions(reactions);
            }
        }

        if 
            (CanWalkDebugMode) DrawDebugMap(map, canWalk);
        else
            DrawMap(map);
        // TODO players shall be able to choose their heroes
        foreach (Player p in players)
        {
            PlaceRandomHero(p, p.Castle[0].GetPosition());
        }

        // Kaster mapmaker
        mapmaker = null;
	}

    private void PlaceBuildings()
    {
        Player ownerOfMines = players[0];
        foreach (Region r in regions)
        {
            OverworldObjects.ResourceBuilding mine = new OreMine(ownerOfMines);
            placement.Place( r, mine );
            ownerOfMines.ResourceBuildings.Add(mine);

            mine = new GemMine(ownerOfMines);
            placement.Place( r, mine);
            ownerOfMines.ResourceBuildings.Add(mine);

            mine = new CrystalMine(ownerOfMines);
            placement.Place( r, mine);
            ownerOfMines.ResourceBuildings.Add(mine);

            mine = new GoldMine(ownerOfMines);
            placement.Place( r, mine);
            ownerOfMines.ResourceBuildings.Add(mine);
        }
    }
    
    /// <summary>
    /// Setting up UI buttons, text and images.
    /// </summary>
    private void GenerateUI()
    {
        // Set UI for first player's heroes and towns
        setOverworldUI(players[WhoseTurn]);

        // Set date text
        GameObject overworldDatePanel = GameObject.Find("OverworldDatePanel");
        GameObject textObject = overworldDatePanel.transform.GetChild(0).gameObject;
        dateText = textObject.GetComponent<Text>();
        dateText.text = date.ToString();

        GameObject overworldResourcePanel = GameObject.Find("OverworldResourcePanel");
        // Set resource text
        resourceText = new Text[overworldResourcePanel.transform.childCount];
        for (int i = 0; i < resourceText.Length; i++)
        {
            resourceText[i] = overworldResourcePanel.transform.GetChild(i).GetChild(0).GetComponent<Text>();
        }
        updateResourceText();
    }

    /// <summary>
    /// Setting resourcepanel's textboxes to the players resources.
    /// </summary>
    public void updateResourceText()
    {
        for (int i = 0; i < resourceText.Length; i++)
        {
            resourceText[i].text = players[WhoseTurn].Wallet.GetResource(i) + "";
        }
    }

	/// <summary>
	/// Draws a given map using the IngameObjectLibrary sprites.
	/// </summary>
	/// <param name="map">Map created by MapMaker.</param>
	protected void DrawMap(int[,] map)
	{
        // Creating the different object categories. Same as sorting layers in the project
            GameObject ground = new GameObject();
            ground.name = "Ground";

            GameObject mountains = new GameObject();
            mountains.name = "Mountains";

            GameObject forests = new GameObject();
            forests.name = "Forest";

            GameObject buildings = new GameObject();
            buildings.name = "Buildings";

            GameObject pickups = new GameObject();
            pickups.name = "Pickups";

            GameObject heroes = new GameObject();
            heroes.name = "Heroes";

        buildingLayer = new GameObject[width, height];
        heroLayer = new GameObject[width, height];

        // DRAWING THE MAP:
        groundLayer = new GameObject[width, height];
		float isometricOffset = 0;

		// Looping through all tile positions:
		for (int y = 0; y < height; y++)
		{

			for (int x = 0; x < width; x++)
			{
                // gets tile value
				int spriteID = map[x, y];

				// If ground
				if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Ground)
				{
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(spriteID), ground);
				}

				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Environment)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetEnvironment(spriteID), mountains);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(MapMaker.GRASS_SPRITEID), ground); //TODO:temp
				}

				// If dwelling
				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Dwellings)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDwelling(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(MapMaker.GRASS_SPRITEID), ground); //TODO:temp
				}

				// If resource buildings
				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.ResourceBuildings)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetResourceBuilding(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(MapMaker.GRASS_SPRITEID), ground); //TODO:temp
				}

				// If hero
				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Heroes)
				{
					heroLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetHero(spriteID), heroes);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(MapMaker.GRASS_SPRITEID), ground);
				}

				// If castle
				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Castle)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetCastle(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(MapMaker.GRASS_SPRITEID), ground); //TODO:temp
				}

				// If debug mode:
				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Debug)
				{
					groundLayer[x,y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(map[x,y]), ground);
				}
                
            }
			isometricOffset += YOFFSET;
		}
	}

	/// <summary>
	/// Draws the debug map.
	/// </summary>
	/// <param name="map">Map.</param>
	/// <param name="canWalk">Can walk.</param>
	void DrawDebugMap(int[,] map, int[,] canWalk)
	{
		GameObject DebugTiles = new GameObject();
		DebugTiles.name = "Debug";

        GameObject mountains = new GameObject();
		mountains.name = "Mountains";

		GameObject forests = new GameObject();
		forests.name = "Forest";

		GameObject buildings = new GameObject();
		buildings.name = "Buildings";

		GameObject pickups = new GameObject();
		pickups.name = "Pickups";

		GameObject heroes = new GameObject();
		heroes.name = "Heroes";

		buildingLayer = new GameObject[width, height];
		heroLayer = new GameObject[width, height];

		// DRAWING THE MAP:
		groundLayer = new GameObject[width, height];
		float isometricOffset = 0;

		// Looping through all tile positions:
		for (int y = 0; y < height; y++)
		{

			for (int x = 0; x < width; x++)
			{
				// gets tile value

				//int spriteID = map[x, height - 1 - y];
				int spriteID = map[x, y];
				// TODO: CURRENT: Denne er snudd

				if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Environment)
				{
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x, y]), DebugTiles); //TODO:temp
				}

				// If dwelling
				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Dwellings)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDwelling(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x,y]), DebugTiles); //TODO:temp
				}

				// If resource buildings
				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.ResourceBuildings)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetResourceBuilding(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x, y]), DebugTiles); //TODO:temp
				}

				// If hero
				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Heroes)
				{
					heroLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetHero(spriteID), heroes);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x, y]), DebugTiles); //TODO:temp
				}

				// If castle
				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Castle)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetCastle(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x, y]), DebugTiles); //TODO:temp
				}

				// If debug mode:
				else if (IngameObjectLibrary.GetCategory(spriteID) == IngameObjectLibrary.Category.Ground)
				{
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x, y]), DebugTiles);
				}

			}
			isometricOffset += YOFFSET;
		}

	}

    /// <summary>
    /// Configures a game object to the board.
    /// </summary>
    /// <param name="x">X postition for logical placement</param>
    /// <param name="y">Y postition for logical placement</param>
    /// <param name="isometricOffset">Offset for isometric presentation</param>
    /// <param name="Sprite">Sprite from libs. </param>
    /// <param name="parent">Parent gameobject</param>
    /// <returns>Configured gameobject</returns>
    GameObject placeSprite(int x, int y, float isometricOffset, Sprite sprite, GameObject parent)
    {
        GameObject gameObject = new GameObject();
        gameObject.tag = parent.tag;
        gameObject.name = parent.name + "(" + x + ", " + y + ")";
        gameObject.transform.position = getIsometricPlacement(x, y, isometricOffset);

        /// Sets building sprite
        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = parent.name;
        sr.sprite = sprite;
        gameObject.transform.parent = parent.transform;

        return gameObject;
    }

    /// <summary>
    /// Adjusts the position relative to odd or par placement.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="isometricOffset"></param>
    /// <returns>Adjusted vector2 postion</returns>
    private Vector2 getIsometricPlacement (int x, int y, float isometricOffset)
    {
        if (y % 2 == 0) // IF PAR
            return new Vector2(x, isometricOffset / 2);
        else // IF ODD
            return new Vector2(x + 0.5f, isometricOffset / 2);
    }

    /// <summary>
    /// Called by UI click on town, activates towngameobjects and deactivates overworld
    /// </summary>
    public void EnterTown(Town town)
    {
        townArmyPanel.SetActive(true);
        DrawTown(town);
        townWindow.SetActive(true);
        overWorld = false;
        cameraMovement.enabled = false;
    }

    /// <summary>
    /// Called by methods that exits the town, deactivates towngameobjects and activates overworld
    /// </summary>
    public void ExitTown()
    {
        townArmyPanel.SetActive(false);
        townWindow.SetActive(false);
        overWorld = true;
        cameraMovement.enabled = true;
        DestroyTownBuildings();
        DeactivateTownPanels();
    }

    /// <summary>
    /// Check all the children in the town canvas, deactivate if any of them are activated
    /// </summary>
    public void DeactivateTownPanels()
    {
        if (buildingPanel.activeSelf)
            buildingPanel.SetActive(false);
        if (dwellingPanel.activeSelf)
            dwellingPanel.SetActive(false);
        if (marketplacePanel.activeSelf)
            marketplacePanel.SetActive(false);
        if (townHallPanel.activeSelf)
            townHallPanel.SetActive(false);
        if (tavernPanel.activeSelf)
            tavernPanel.SetActive(false);
        if (adjustResourcePanel.activeSelf)
            adjustResourcePanel.SetActive(false);
    }

    /// <summary>
    /// Checks and destroys all objects in towntodestroypanel
    /// </summary>
    public void DestroyTownBuildings()
    {
        for (int i = 0; i < townToDestroyPanel.transform.childCount; i++)
            Destroy(townToDestroyPanel.transform.GetChild(i).gameObject);
    }

    /// <summary>
    /// Draws the town view
    /// </summary>
    /// <param name="town"></param>
    public void DrawTown(Town town)
    {
        float scaleFactor = 0.45f; //TODO: regn ut fra skjerm

        // Sets up the town view background
        SpriteRenderer sr = townWindow.GetComponent<SpriteRenderer>();
        sr.sprite = libs.GetTown(town.GetSpriteID());
        townWindow.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
        sr.sortingLayerName = "TownWindow";

        // Creates a GameObject array for the new building
        buildingsInActiveTown = new GameObject[town.Buildings.Length];

        town.BuildAll(town);
        town.Buildings[9].Build();

        // loads in the town buildings
        for (int i = 0; i < town.Buildings.Length; i++)
        {
            // If the building is built, draw it 
            if (town.Buildings[i].Built)
            {
                DrawBuilding(town, town.Buildings[i], i);
            }
        }

        // Draw bar for town's heroes and units
        DrawTownArmy(town);
    }

    /// <summary>
    /// Draws the clicked buidling in town
    /// </summary>
    /// <param name="town">town the building is connected to</param>
    /// <param name="building">which building clicked</param>
    /// <param name="i">the position of the building in the buildingarray</param>
    public void DrawBuilding(Town town, Building building, int i)
    {
        float scaleFactor = 0.45f; //TODO: regn ut fra skjerm

        // Gets parent X,Y and uses offset coords to draw in place
        Vector2 placement = new Vector2(
            townWindow.transform.position.x,
            townWindow.transform.position.y
        );
        // Creates a game object for the building, gives it a name and places and scales it properly
        string prefabPath = "Prefabs/" + town.Buildings[i].Name;
        buildingsInActiveTown[i] = Instantiate(UnityEngine.Resources.Load<GameObject>(prefabPath));
        buildingsInActiveTown[i].transform.parent = townToDestroyPanel.transform;
        //buildingsInActiveTown[i].transform.localScale = Vector3.one;
        buildingsInActiveTown[i].transform.position = placement;
        //buildingsInActiveTown[i].transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        // CONNECTING GAMEOBJECT WITH BUILDING OBJECT: 
        buildingsInActiveTown[i].GetComponent<BuildingOnClick>().Building = town.Buildings[i];
        buildingsInActiveTown[i].GetComponent<BuildingOnClick>().BuildingObjects = buildingsInActiveTown;
        buildingsInActiveTown[i].GetComponent<BuildingOnClick>().Town = town;
        buildingsInActiveTown[i].GetComponent<BuildingOnClick>().Player = players[WhoseTurn];
    }

    public void DestroyBuildingsInTown()
    {

    }

    /// <summary>
    /// Sets UI for the startplayers hero and towns to click on
    /// </summary>
    /// <param name="player">The player that starts</param>
    private void setOverworldUI(Player player)
    {
        GameObject overworldTownPanel = GameObject.Find("OverworldTownPanel");
        GameObject overworldHeroPanel = GameObject.Find("OverworldHeroPanel");
        heroObjects = new GameObject[overworldHeroPanel.transform.childCount];
        heroButton = new Button[heroObjects.Length];
        // Heroes
        for (int i = 0; i < heroObjects.Length; i++)
        {
            // Initialize all the gameobjects with the buttons
            heroObjects[i] = overworldHeroPanel.transform.GetChild(i).gameObject;
            heroButton[i] = heroObjects[i].GetComponent<Button>();
        }
        townObjects = new GameObject[overworldTownPanel.transform.childCount];
        townButton = new Button[townObjects.Length];
        // Towns
        for (int i = 0; i < townObjects.Length; i++)
        {
            // Initialize all the gameobjects with the buttons
            townObjects[i] = overworldTownPanel.transform.GetChild(i).gameObject;
            townButton[i] = townObjects[i].GetComponent<Button>();
        }
        updateOverworldUI(player);
    }

    /// <summary>
    /// Updates the UI to the current players heroes and towns
    /// </summary>
    /// <param name="player">current player</param>
    public void updateOverworldUI(Player player)
    {
        // Heroes
        for (int i = 0; i < heroObjects.Length; i++)
        {
            // Sets the button properties if it finds a hero
            if (i < player.Heroes.Length && player.Heroes[i] != null)
            {
                Sprite sprite = libs.GetPortrait(player.Heroes[i].GetPortraitID());

                heroButton[i].GetComponent<Image>().sprite = sprite;
                GameObject swapObject = heroObjects[i];
                Vector2 position = HandyMethods.getGraphicPosForIso(player.Heroes[i].Position.ToVector2());
                Hero hero = player.Heroes[i];
                heroButton[i].onClick.AddListener(() => cameraMovement.centerCamera(position));
                heroButton[i].onClick.AddListener(() => activeHero = hero);
                heroButton[i].onClick.AddListener(() => activeHeroObject = heroLayer[activeHero.Position.x, activeHero.Position.y]);

            }
            else
                heroObjects[i].SetActive(false);
        }
        // Towns
        for (int i = 0; i < townObjects.Length; i++)
        {
            // Sets the button properties if it finds a castle
            if (i < player.Castle.Count && player.Castle[i] != null)
            {
                Sprite sprite = libs.GetTown(player.Castle[i].Town.GetSpriteID());
                townButton[i].GetComponent<Image>().sprite = sprite;
                Castle castle = player.Castle[i];
                townButton[i].onClick.AddListener(() => EnterTown(castle.Town));
            }
            else
                townObjects[i].SetActive(false);
        }
    }

    /// <summary>
    /// Draws the hero and units in visiting and stationed garrison
    /// </summary>
    /// <param name="town">the town with the heroes and units</param>
    public void DrawTownArmy(Town town)
    {
        Sprite defaultsprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/NoUnit");
        
        // 7 + 7 units + 2 heroes in town view
        armyInActiveTown = new GameObject[16];
        swapObject = null;

        int count = 0;

        // Draws visitinghero with listener for swapping
        armyInActiveTown[count] = townArmyPanel.transform.GetChild(count).gameObject;
        Button visitingHeroButton = armyInActiveTown[count].GetComponent<Button>();
        visitingHeroButton.GetComponent<Image>().sprite = defaultsprite;
        GameObject swapVisitingHero = armyInActiveTown[count];
        visitingHeroButton.onClick.AddListener(() => SwapArmy(swapVisitingHero, town));
        if (town.VisitingHero != null)
            visitingHeroButton.GetComponent<Image>().sprite = libs.GetPortrait(town.VisitingHero.GetPortraitID());
        count++;

        // Draws visitingunits with listener for swapping
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            armyInActiveTown[count] = townArmyPanel.transform.GetChild(count).gameObject;
            Button visitingUnitButton = armyInActiveTown[count].GetComponent<Button>();
            visitingUnitButton.GetComponent<Image>().sprite = defaultsprite;
            GameObject swapVisitingUnit = armyInActiveTown[count];
            visitingUnitButton.onClick.AddListener(() => SwapArmy(swapVisitingUnit, town));
            Text text = armyInActiveTown[count].transform.GetChild(0).GetComponent<Text>();
            if (town.VisitingUnits.GetUnits()[i] != null)
            {
                visitingUnitButton.GetComponent<Image>().sprite = libs.GetUnit(town.VisitingUnits.GetUnits()[i].GetSpriteID());
                text.text = town.VisitingUnits.getUnitAmount(i) + "";
            }
            else
                text.text = "";
            count++;
        }

        // Draws stationedHero with listener for swapping
        armyInActiveTown[count] = townArmyPanel.transform.GetChild(count).gameObject;
        Button stationedHeroButton = armyInActiveTown[count].GetComponent<Button>();
        stationedHeroButton.GetComponent<Image>().sprite = defaultsprite;
        GameObject swapStationedHero = armyInActiveTown[count];
        stationedHeroButton.onClick.AddListener(() => SwapArmy(swapStationedHero, town));
        if (town.StationedHero != null)
            stationedHeroButton.GetComponent<Image>().sprite = libs.GetPortrait(town.VisitingHero.GetPortraitID());
        count++;

        // Draws stationedUnits with listener for swapping
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            armyInActiveTown[count] = townArmyPanel.transform.GetChild(count).gameObject;
            Button stationedUnitButton = armyInActiveTown[count].GetComponent<Button>();
            stationedUnitButton.GetComponent<Image>().sprite = defaultsprite;
            GameObject swapStatonedUnit = armyInActiveTown[count];
            stationedUnitButton.onClick.AddListener(() => SwapArmy(swapStatonedUnit, town));
            Text text = armyInActiveTown[count].transform.GetChild(0).GetComponent<Text>();
            if (town.VisitingUnits.GetUnits()[i] != null)
            {
                stationedUnitButton.GetComponent<Image>().sprite = libs.GetUnit(town.StationedUnits.GetUnits()[i].GetSpriteID());
                text.text = town.StationedUnits.getUnitAmount(i) + "";
            }
            else
                text.text = "";
            count++;
        }
    }
    
    /// <summary>
    /// Updates the heroes and units in the town
    /// </summary>
    /// <param name="town">the town to update the heroes and units</param>
    public void ReDrawArmyInTown(Town town)
    {
        Sprite defaultSprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/NoUnit");
        int count = 0;

        // Sets hero sprite depending on which hero is in town
        if (town.VisitingHero != null)
            armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = libs.GetPortrait(town.VisitingHero.GetPortraitID());
        else
            armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = defaultSprite;


        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            Text text = armyInActiveTown[count].transform.GetChild(0).GetComponent<Text>();
            if (town.VisitingUnits != null && town.VisitingUnits.GetUnits()[i] != null)
            {
                text.text = town.VisitingUnits.getUnitAmount(i) + "";
                armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = libs.GetUnit(town.VisitingUnits.GetUnits()[i].GetSpriteID());
            }
            else
            {
                text.text = "";
                armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = defaultSprite;
            }
        }
        if (town.StationedHero != null)
            armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = libs.GetPortrait(town.StationedHero.GetPortraitID());
        else
            armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = defaultSprite;
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            Text text = armyInActiveTown[count].transform.GetChild(0).GetComponent<Text>();
            if (town.StationedUnits != null && town.StationedUnits.GetUnits()[i] != null)
            {
                text.text = town.StationedUnits.getUnitAmount(i) + "";
                armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = libs.GetUnit(town.StationedUnits.GetUnits()[i].GetSpriteID());
            }
            else
            {
                text.text = "";
                armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = defaultSprite;
            }
        }
    }

    /// <summary>
    /// Method to swap units or heroes in a town screen, both visually and logically
    /// </summary>
    /// <param name="gameObject">The pressed object to swap</param>
    /// <param name="town">The town in which the swap is happening</param>
    public void SwapArmy(GameObject gameObject, Town town)
    {
        // If there is a unit or hero there, check if you can swap it
        if (swapObject != null)
        {
            if(swapObject.name.Equals(gameObject.name))
            {
                swapObject = null;
                // TODO Open the units/heroes menu
            }
            else
            {
                // Check if a hero was chicked or hero was already activated
                if (gameObject.name.Equals("VisitingHero") || gameObject.name.Equals("StationaryHero") || swapObject.name.Equals("VisitingHero") || swapObject.name.Equals("StationaryHero"))
                {
                    // Check if a hero was activated and another hero was clicked
                    if ((gameObject.name.Equals("VisitingHero") && swapObject.name.Equals("StationaryHero")) || (gameObject.name.Equals("StationaryHero") && swapObject.name.Equals("VisitingHero")))
                    {
                        // Swap heroes logically (also swaps the towns.unittree's)
                        town.swapHeroes();
                        // Redraw visuals
                        ReDrawArmyInTown(town);
                        // In the map, hide stationaryhero, show visitinghero
                        //refreshTownHeroes(town);

                        swapObject = null;
                    }
                    // If not hero swap, just activate the newly clicked gameobject
                    else
                    {
                        // TODO dont activate if no unit/hero at position of gameobject
                        swapObject = gameObject;
                    }
                }
                else
                {
                    // VISITING 0-6, STATIONARY 7-13
                    int unitPos1 = Int32.Parse(swapObject.name);
                    int unitPos2 = Int32.Parse(gameObject.name);
                    // Both stationary
                    if (unitPos1 >= UnitTree.TREESIZE && unitPos2 >= UnitTree.TREESIZE)
                    {
                        unitPos1 -= UnitTree.TREESIZE;
                        unitPos2 -= UnitTree.TREESIZE;
                        town.SwapStationaryUnits(unitPos1, unitPos2);
                    }
                    else if(unitPos1 >= UnitTree.TREESIZE)
                    {
                        unitPos1 -= UnitTree.TREESIZE;
                        town.SwapStationedVisitingUnits(unitPos1, unitPos2);
                    }
                    else if(unitPos2 >= UnitTree.TREESIZE)
                    {
                        unitPos2 -= UnitTree.TREESIZE;
                        town.SwapVisitingStationaryUnits(unitPos1, unitPos2);
                    }
                    else
                    {
                        town.SwapVisitingUnits(unitPos1, unitPos2);
                    }

                    // Swap logically with int positions
                    //town.swapUnits(unitPos1, unitPos2);
                    // Redraw visuals
                    ReDrawArmyInTown(town);
                    swapObject = null;
                    // TODO Swap two units, også ta for seg swapping mellom stationared og visitingunits, med forbehold om at det finnes en hero i visiting
                }
            }
        }
        // If there wasn't a unit or hero there, just activate the one pressed
        else
        {
            // TODO dont activate if no unit/hero at position of gameobject

            // VISITING 0-6, STATIONARY 7-13
            if (gameObject.name.Equals("VisitingHero") || gameObject.name.Equals("StationaryHero"))
            {
                if ((gameObject.name.Equals("VisitingHero") && town.VisitingHero != null) || (gameObject.name.Equals("StationaryHero") && town.StationedHero != null))
                    swapObject = gameObject;
            }
            else
            {
                int unitpos = Int32.Parse(gameObject.name);
                if(((unitpos >= UnitTree.TREESIZE) && town.StationedUnits != null && town.StationedUnits.GetUnits()[unitpos - UnitTree.TREESIZE] != null) || ((unitpos < UnitTree.TREESIZE) && town.VisitingUnits != null && town.VisitingUnits.GetUnits()[unitpos] != null))
                    swapObject = gameObject;
            }
        }
    }

    public void refreshTownHeroes(Town town)
    {
        if (town.StationedHero != null)
            heroLayer[town.StationedHero.Position.x, town.StationedHero.Position.y].SetActive(false);
        if (town.VisitingHero != null)
            heroLayer[town.VisitingHero.Position.x, town.VisitingHero.Position.y].SetActive(true);
    }

    public void showHero(Hero hero)
    {
        heroLayer[hero.Position.x, hero.Position.y].SetActive(true);
    }

    public void hideHero(Hero hero)
    {
        heroLayer[hero.Position.x, hero.Position.y].SetActive(false);
    }
    

    /// <summary>
    /// Called by next turn UI button
    /// </summary>
    public void nextTurn()
    {
        // Stop ongoing movement
        if(movement.walking)
        {
            movement.lastStep = false;
        }
        else
        {
            // Refresh movementspeed for the upcoming players heroes
            foreach (Hero hero in players[WhoseTurn].Heroes)
            {
                if (hero != null)
                    hero.CurMovementSpeed = hero.MovementSpeed;
            }
            // Remove all path markers on the map
            movement.RemoveMarkers(movement.pathObjects);

            // Set active hero and active hero object to the upcoming players first hero

            // On next turn, always set the next player as active player. But if next player 
            // does not exist, keep incrementing till you find an exisiting player.
            do
            {
                incrementTurn();
            }
            while (players[WhoseTurn] == null);


            if(players[WhoseTurn].Heroes[0] != null)
            {
               activeHero = players[WhoseTurn].Heroes[0];
               movement.activeHero = activeHero;

               activeHeroObject = heroLayer[activeHero.Position.x, activeHero.Position.y];
                if (movement.activeHero.Path != null && movement.activeHero.Path.Count > 0)
                {
                    movement.MarkPath(movement.activeHero.Path[movement.activeHero.Path.Count-1]);
                }
               // Center camera to the upcoming players first hero
               cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(activeHero.Position));
            }
            else
            {
                activeHero = null;
                movement.activeHero = null;
                activeHeroObject = null;
                // Center camera to the upcoming players first castle
                cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(players[WhoseTurn].Castle[0].GetPosition().ToVector2()));
            }

            // Gather income for the upcoming player
            players[WhoseTurn].GatherIncome();
            

            // Update wallet UI
            updateResourceText();
            // Update herolist and townlist UI
            updateOverworldUI(players[WhoseTurn]);
        }
    }

    ///  <summary>
    /// Increment turn, reset turn integer when last player has finished, and increment date
    /// </summary>
    private void incrementTurn()
    {
        if (++WhoseTurn >= amountOfPlayers)
        {
            WhoseTurn = 0;
            // Update date text
            dateText.text = date.incrementDay();
            updateCanBuild();
            players[WhoseTurn].PopulateDwellings();
        }
    }

    /// <summary>
    /// On turn incrementation, the next player's towns hasbuilt is refreshed.
    /// </summary>
    private void updateCanBuild()
    {
        for (int i = 0; i < players.Length; i++)
        {
            foreach(Castle castle in players[i].Castle)
            {
                castle.Town.HasBuiltThisRound = false;
            }
        }
    }

    public void enterCombat(int width, int height, Hero attacker, Hero defender)
    {
        overWorld = false;
        //todo add canvas
        // graphicalBattlefield.beginCombat(width, height, attacker, defender)
    }

    public void enterCombat(int width, int height, Hero attacker, UnitTree defender)
    {
        overWorld = false;
        combatWindow.SetActive(true);
        graphicalBattlefield.beginCombat(width, height, attacker, defender);
        cameraMovement.enabled = false;
        combatWindow.transform.localPosition = new Vector3(0,0,10);
    }

    public void exitCombat(bool winner)
    {
        if (winner)
        {
            //attacker won
            if (curReaction.GetType() == typeof(CastleReact))
            {
                CastleReact cr = (CastleReact) curReaction;
                changeCastleOwner(cr);
            }
        }
        else
        {
            //defender won
            removeHero(activeHero);
        }
        combatWindow.SetActive(false);
        overWorld = true;
        cameraMovement.enabled = true;
    }

    /// <summary>
    /// Changes owner of castle to Player whose turn it is
    /// </summary>
    /// <param name="cr">CastleReact</param>
    public void changeCastleOwner(CastleReact cr)
    {
        cr.Castle.Player.Castle.Remove(cr.Castle);
        cr.Castle.Player = players[WhoseTurn];
        cr.Castle.Town.Owner = players[WhoseTurn];
        players[WhoseTurn].Castle.Add(cr.Castle);
    }


    /// <summary>
    /// Removes a hero
    /// </summary>
    /// <param name="h">Hero to be removed</param>
    public void removeHero(Hero hero)
    {
        hero.Player.removeHero(hero);

        // Find the hero in the games' herolist, set alive false
        foreach(Hero h in heroes)
        {
            if (h.Equals(hero))
                h.Alive = false;
        }

        if (activeHero.Equals(hero))
            activeHero = null;

        // Remove visually
        GameObject go = heroLayer[hero.Position.x, hero.Position.y];
        go.SetActive(false);
        Destroy(go);
        heroLayer[hero.Position.x, hero.Position.y] = null;

        // Remove the reaction, either main reaction or prereaction
        if (reactions[hero.Position.x, hero.Position.y].GetType() == (typeof(HeroMeetReact)))
            reactions[hero.Position.x, hero.Position.y] = null;
        else
            reactions[hero.Position.x, hero.Position.y].PreReaction = null;
    }

    /// <summary>
    /// Looks randomly through the hero table, finds a hero that is not alive and calls the placehero method, with the 
    /// players first castle as position
    /// </summary>
    /// <param name="player">The player that gets the new hero</param>
    public void PlaceRandomHero(Player player, Point position)
    {
        bool placed = false;
        while (!placed)
        {
            int random = UnityEngine.Random.Range(0, heroes.Length);
            if (!heroes[random].Alive)
            {
                PlaceHero(player, heroes[random], position);
                placed = true;
            }
        }
    }
    
    /// <summary>
    /// Places the parameter hero logically and visually
    /// </summary>
    /// <param name="player">Player that gets the hero</param>
    /// <param name="hero">The hero to place</param>
    /// <param name="position">Where to place him</param>
    public void PlaceHero(Player player, Hero hero, Point position)
    {
        // Create the visual object
        GameObject heroObject = new GameObject();
        heroObject.AddComponent<SpriteRenderer>().sprite = libs.GetHero(hero.GetSpriteID());
        // Add the visual object to the game
        heroObject.name = "Heroes";
        Vector2 a = getIsometricPlacement(position.x, position.y, position.y);
        Point isometricPosition = new Point((int)a.x, (int)a.y);
        heroLayer[position.x, position.y] = placeSprite(position.x, position.y, isometricPosition.y, libs.GetHero(hero.GetSpriteID()), heroObject);
        // Add the hero to a castle if the position corresponds with a castle of the players ownership
        for(int i=0; i<player.Castle.Count; i++)
            if(player.Castle[i].GetPosition().Equals(position) && player.Castle[i].Town.VisitingHero == null)
                    player.Castle[i].Town.VisitingHero = hero;
        // Add hero to corresponding player
        player.addHero(hero, position);
        // Flip canwalk
        canWalk[position.x, position.y] = 2;
        // if there's already a castle at the position, place the heroreact under it, else place it normally in reaction tab
        if (reactions[position.x, position.y] != null)
            reactions[position.x, position.y].PreReaction = new HeroMeetReact(hero, position);
        else
            reactions[position.x, position.y] = new HeroMeetReact(hero, position);
    }

    /// <summary>
    /// Writes a units details on top of a unit playing card
    /// </summary>
    /// <param name="parent">The parent of the unit card</param>
    /// <param name="canvas">The canvas the unit card is attached to</param>
    /// <param name="unitBuilding">The unit building which has all the information on the unit to display</param>
    // TODO: extend method so that this method creates teh unit card backgrounda s well
    public void CreateUnitCard(GameObject parent, GameObject canvas, UnitBuilding unitBuilding)
    {

        BuildingCard card = new BuildingCard(WindowTypes.UNIT_CARD, IngameObjectLibrary.Category.UI);

        // Creates a building card game ojbect with a spriterenderer, sets its position, layer, name and parent
        GameObject cardWindow = new GameObject();
        cardWindow.transform.parent = parent.transform;
        cardWindow.name = "TownCardPanel";
        cardWindow.tag = "toDestroy";
        cardWindow.transform.position = cardWindow.transform.parent.position;
        SpriteRenderer cardSpriteRenderer = cardWindow.AddComponent<SpriteRenderer>();
        cardSpriteRenderer.sprite = libs.GetUI(card.GetSpriteID());
        cardSpriteRenderer.sortingLayerName = "TownGUI";

        int pos = 0;

        string unitName = unitBuilding.GetUnitName();
        string unitAttack = unitBuilding.GetAttack() + "";
        string unitDefense = unitBuilding.GetDefense() + "";
        string unitMagic = unitBuilding.GetMagic() + "";
        string unitSpeed = unitBuilding.GetSpeed() + "";
        Ability unitAbility = unitBuilding.GetAbility();
        Move[] moves = unitBuilding.GetMoves();
        int unitPortrait = unitBuilding.GetImage();
        
        // Unit portrait
        GameObject unitPortraitObject = new GameObject();
        unitPortraitObject.name = unitName + " portrait";   
        unitPortraitObject.transform.position = parent.transform.position;
        unitPortraitObject.transform.localScale = canvas.transform.localScale;
        unitPortraitObject.transform.parent = parent.transform;
        SpriteRenderer sr = unitPortraitObject.AddComponent<SpriteRenderer>();
        sr.sprite = libs.GetUnit(unitPortrait);
        sr.sortingLayerName = "TownGUI";


        // Name of unit
        GameObject unitNameObject = new GameObject();
        SetUnitCardText(unitNameObject, parent, canvas, unitName, pos++, BIG_FONT);

        // attack of unit
        GameObject unitAttackObject = new GameObject();
        SetUnitCardText(unitAttackObject, parent, canvas, unitAttack, pos++, BIG_FONT);

        // defence of unut
        GameObject unitDefenseObject = new GameObject();
        SetUnitCardText(unitDefenseObject, parent, canvas, unitDefense, pos++, BIG_FONT);

        // magic of unit
        GameObject unitMagicObject = new GameObject();
        SetUnitCardText(unitMagicObject, parent, canvas, unitMagic, pos++, BIG_FONT);

        // speed of unit
        GameObject unitSpeedObject = new GameObject();
        SetUnitCardText(unitSpeedObject, parent, canvas, unitSpeed, pos++, BIG_FONT);

        // ability name and description of unit
        GameObject unitAbilityNameObject = new GameObject();
        SetUnitCardText(unitAbilityNameObject, parent, canvas, unitAbility.Name, pos++, BIG_FONT);
        GameObject unitAbilityDescriptionObject = new GameObject();
        SetUnitCardText(unitAbilityDescriptionObject, parent, canvas, unitAbility.Description, pos++, SMALL_FONT);

        // move 1 name, description and dmg of unit
        string moveName = moves[0].Name;
        string moveDesc = moves[0].Description;
        string moveDamage = moves[0].MinDamage + "-" + moves[0].MaxDamage;

        GameObject unitMove1NameObject = new GameObject();
        SetUnitCardText(unitMove1NameObject, parent, canvas, moveName, pos++, BIG_FONT);
        GameObject unitMove1DescriptionObject = new GameObject();
        SetUnitCardText(unitMove1DescriptionObject, parent, canvas, moveDesc, pos++, SMALL_FONT);
        GameObject unitMove1DamageObject = new GameObject();
        SetUnitCardText(unitMove1DamageObject, parent, canvas, moveDamage, pos++, BIG_FONT);

        // move 2 name, description and dmg of unit
        moveName = moves[1].Name;
        moveDesc = moves[1].Description;
        moveDamage = moves[1].MinDamage + "-" + moves[1].MaxDamage;

        GameObject unitMove2NameObject = new GameObject();
        SetUnitCardText(unitMove2NameObject, parent, canvas, moveName, pos++, BIG_FONT);
        GameObject unitMove2DescriptionObject = new GameObject();
        SetUnitCardText(unitMove2DescriptionObject, parent, canvas, moveDesc, pos++, SMALL_FONT);
        GameObject unitMove2DamageObject = new GameObject();
        SetUnitCardText(unitMove2DamageObject, parent, canvas, moveDamage, pos++, BIG_FONT);

    }


    private static float UNIT_CARD_TEXT_MIDDLE = 0.11f;
    private static float UNIT_CARD_TEXT_RIGHT = -0.9f;
    private static float UNIT_CARD_TEXT_LEFT = 2.14f;
    private static float UNIT_CARD_MIDDLE_LEFT = 1f;
    /// <summary>
    /// Text positions relative to its parent object
    /// </summary>
    private readonly Vector2[] unitCardTextPos =
    {
        // TODO: polish the positioning of text
        new Vector2(UNIT_CARD_TEXT_MIDDLE, 2.34f), // 0, name
        new Vector2(UNIT_CARD_TEXT_LEFT, -1.25f), // 1, attack
        new Vector2(UNIT_CARD_TEXT_LEFT, -1.9f), // 2, defense
        new Vector2(UNIT_CARD_TEXT_LEFT, -2.5f), // 3, magic
        new Vector2(UNIT_CARD_TEXT_LEFT, -3.1f), // 4, speed
        new Vector2(UNIT_CARD_TEXT_RIGHT, -3f), // 5, ability name
        new Vector2(UNIT_CARD_TEXT_RIGHT, -3.4f), // 6, ability desc
        new Vector2(UNIT_CARD_TEXT_RIGHT, -1f), // 7, move1 name 
        new Vector2(UNIT_CARD_TEXT_RIGHT, -1.2f), // 8, move1 desc
        new Vector2(UNIT_CARD_MIDDLE_LEFT, -1.25f), // 9, move1 dmg
        new Vector2(UNIT_CARD_TEXT_RIGHT, -1.8f), // 10, move2 name 
        new Vector2(UNIT_CARD_TEXT_RIGHT, -2f), // 11, move2 desc
        new Vector2(UNIT_CARD_MIDDLE_LEFT, -2.2f), // 12, move2 dmg
    };

    private int BIG_FONT = 15;
    private int SMALL_FONT = 10;

    /// <summary>
    /// Method to attach text to a unit playing card
    /// </summary>
    /// <param name="toAttach">The game object to attach text to</param>
    /// <param name="parent">The parent of toAttach</param>
    /// <param name="canvas">The canvas toAttach is a part of</param>
    /// <param name="text">The text to write</param>
    /// <param name="pos">The position of the text relative to its parent, taken from unitCardTextPos</param>
    /// <param name="fontSize">The size of the font displayed</param>
    public void SetUnitCardText(GameObject toAttach, GameObject parent, GameObject canvas, string text, int pos, int fontSize)
    {
        toAttach.transform.parent = parent.transform;
        toAttach.transform.position = (Vector2) parent.transform.position + unitCardTextPos[pos];
        toAttach.transform.localScale = canvas.transform.localScale;
        toAttach.name = text + " text";
        Text unitText = toAttach.AddComponent<Text>();
        unitText.text = text;
        unitText.font = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");
        unitText.fontSize = fontSize;
        unitText.color = Color.black;


        // TODO: fix rect tansform for long texts
        /*
        // Alternativ 2
        HorizontalLayoutGroup layout = toAttach.AddComponent<HorizontalLayoutGroup>();
        layout.preferredWidth = 100f;
        ContentSizeFitter fitter = toAttach.AddComponent<ContentSizeFitter>();
        fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
       

        // Alternativ 1
        unitText.alignment = TextAnchor.UpperLeft;
        RectTransform rectTransform = toAttach.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(200f, 200f); */
    }

    public void SetUnitCard(GameObject parent, Unit unit)
    {
        const int MAX_MOVES = 3;
        // Set unitpanel unittext and unitimage

        unitPanel.transform.parent = parent.transform;
        unitPanel.GetComponent<RectTransform>().sizeDelta = new Vector3(0,0,0);
        unitPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = unit.Name;
        unitPanel.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = libs.GetUnit(unit.GetSpriteID());

        GameObject panel = unitPanel.transform.GetChild(2).gameObject;

        GameObject parentMovePanel = panel.transform.GetChild(0).gameObject;

        // Set move info
        for(int i=0; i<MAX_MOVES; i++)
        {
            GameObject movePanel = parentMovePanel.transform.GetChild(i).gameObject;
            if (i < unit.Moves.Length && unit.Moves[i] != null)
            {
                Move move = unit.Moves[i];
                movePanel.transform.GetChild(0).GetComponent<Text>().text = move.Name + "";
                movePanel.transform.GetChild(1).GetComponent<Text>().text = move.MinDamage + " - " + move.MaxDamage; // TODO base minmax?
                //movePanel.transform.GetChild(2).GetComponent<Image>().sprite = libs.getsprite corresponding move?;
                movePanel.transform.GetChild(3).GetComponent<Text>().text = move.Description + "";
                movePanel.SetActive(true);
            }
            else
                movePanel.SetActive(false);
        }

        // Set stats info
        GameObject statsPanel = panel.transform.GetChild(1).gameObject;

        UnitStats unitStats = unit.Unitstats;
        GameObject healthObject = statsPanel.transform.GetChild(0).gameObject;
        //healthObject.GetComponent<Image>().sprite =  // TODO set icon
        string statText = unitStats.BaseHealth + "";
        if (unitStats.BonusHealth > 0)
            statText += " (" + unitStats.Health + ")";
        healthObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = statText;

        GameObject attackObject = statsPanel.transform.GetChild(1).gameObject;
        //attackObject.GetComponent<Image>().sprite =  // TODO set icon
        statText = unitStats.BaseAttack + "";
        if(unitStats.BonusAttack > 0)
            statText += " (" + unitStats.Attack + ")";
        attackObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = statText;

        GameObject defenseObject = statsPanel.transform.GetChild(2).gameObject;
        //attackObject.GetComponent<Image>().sprite =  // TODO set icon
        statText = unitStats.BaseDefence + "";
        if (unitStats.BonusDefence > 0)
            statText += " (" + unitStats.Defence + ")";
        defenseObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = statText;
        
        GameObject speedObject = statsPanel.transform.GetChild(3).gameObject;
        //attackObject.GetComponent<Image>().sprite =  // TODO set icon
        statText = unitStats.BaseSpeed + "";
        if (unitStats.BonusSpeed > 0)
            statText += " (" + unitStats.Speed + ")";
        speedObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = statText;

        GameObject initiativeObject = statsPanel.transform.GetChild(4).gameObject;
        //attackObject.GetComponent<Image>().sprite =  // TODO set icon
        statText = unitStats.BaseInitative + "";
        if (unitStats.BonusInitiative > 0)
            statText += " (" + unitStats.Initative + ")";
        initiativeObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = statText;

        GameObject moralObject = statsPanel.transform.GetChild(5).gameObject;
        //attackObject.GetComponent<Image>().sprite =  // TODO set icon
        statText = unitStats.BaseMoral + "";
        if (unitStats.BonusMoral > 0)
            statText += " (" + unitStats.Moral + ")";
        moralObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = statText;

        GameObject luckObject = statsPanel.transform.GetChild(6).gameObject;
        //attackObject.GetComponent<Image>().sprite =  // TODO set icon
        statText = unitStats.BaseLuck + "";
        if (unitStats.BonusLuck > 0)
            statText += " (" + unitStats.Luck + ")";
        luckObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = statText;

        if(unit.IsRanged)
        {
            Ranged rangedUnit = (Ranged)unit;
            GameObject ammoObject = statsPanel.transform.GetChild(7).gameObject;
            //attackObject.GetComponent<Image>().sprite =  // TODO set icon
            statText = rangedUnit.MaxAmmo + "";
            //if (rangedUnit.BonusAmmo > 0)
            //    statText += " (" + unitStats.Ammo + ")"; // TODO bonus ammo?
            ammoObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = statText;

            GameObject rangeObject = statsPanel.transform.GetChild(8).gameObject;
            //attackObject.GetComponent<Image>().sprite =  // TODO set icon
            statText = unitStats.EffectiveRange + "";
            rangeObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = statText;
        }

        /*
        for(int i=0; i<MAX_STATS; i++)
        {
            if(unit.Unitstats.Health)
        }
        */

        GameObject abilityPanel = panel.transform.GetChild(2).gameObject;
        
        unitPanel.SetActive(true);
    }
}
