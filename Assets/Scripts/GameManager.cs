﻿using UnityEngine;
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
    CameraMovement cameraMovement;

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
    Reaction[,] reactions;

    // Graphical elements
    GameObject[,] groundLayer;
    GameObject[,] buildingLayer;
    GameObject[,] heroLayer;

    // GameManager
    int amountOfPlayers;
    Player[] players;
    int whoseTurn;
    Date date;

    // Click listeners
    const int CLICKSPEED = 35;
    bool prepareDoubleClick;
    int clickCount;
    Vector2 savedClickedPos;


    // Hero movement
    bool heroActive;
    public Hero activeHero;
    GameObject activeHeroObject;
    Sprite pathDestYes;
    Sprite pathDestNo;
    Sprite pathYes;
    Sprite pathNo;
    GameObject parentToMarkers;
    List<GameObject> pathObjects;
    bool pathMarked;
    int stepNumber;
    [Range(0.01f, 1f)]
    public float animationSpeed = 0.01f;
    bool walking;
    bool lastStep;
    int tilesWalking;
    bool newStep;
    Point destination;

    public Hero[] heroes = new Hero[5];

    // Town
    GameObject[] buildingsInActiveTown;
    GameObject[] armyInActiveTown;
    GameObject townArmyCanvas;
    GameObject townWindow;
    public GameObject swapObject;
    bool overWorld;

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
    private Font FONT;
    GameObject[] stackText;

    //currentReaction
    private Reaction curReaction;

    //Comabt
    private GraphicalBattlefield graphicalBattlefield;
    private GameObject combatWindow;

    // Use this for initialization
    void Start ()
    {
        heroes[0] = new Blueberry();
        heroes[1] = new Gork();
        heroes[2] = new JackMcBlackwell();
        heroes[3] = new JohnyMudbone();
        heroes[4] = new Mantooth();

        heroObjects = new GameObject[8];
        townObjects = new GameObject[8];
        heroButton = new Button[8];
        townButton = new Button[8];
        stackText = new GameObject[14];

        // 7 + 7 units + 2 heroes in town view
        armyInActiveTown = new GameObject[16];
        townArmyCanvas = GameObject.Find("ArmyCanvas");
        swapObject = null;
        overworldInteractablePanel = GameObject.Find("OverworldInteractablePanel");
        FONT = UnityEngine.Resources.Load<Font>("Fonts/ARIAL");

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

        pathDestYes = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerDestYes");
        pathDestNo = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerDestNo");
        pathYes = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerPathYes");
        pathNo = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerPathNo");

        // Creating the camera game object and variables
        GameObject tempCameraObject = GameObject.Find("Main Camera");
        mainCamera = tempCameraObject.GetComponent<Camera>();
        cameraMovement = tempCameraObject.GetComponent<CameraMovement>();

        // Set active Hero
        heroActive = true;
        activeHero = getPlayer(0).Heroes[0];
        activeHeroObject = heroLayer[activeHero.Position.x, activeHero.Position.y];

        // Initialize turn based variables and date
        whoseTurn = 0;
        clickCount = 0;
        date = new Date();

        //savedClickedPos = HandyMethods.getIsoTilePos(transform.position);
        pathObjects = new List<GameObject>();
        aStar = new AStarAlgo(canWalk, width, height, false);
        townWindow = GameObject.Find("Town");
        townWindow.SetActive(false);
        overWorld = true;
        GenerateUI();

        //fetch Combat references
        combatWindow = GameObject.Find("Combat");
        combatWindow.SetActive(false);
        graphicalBattlefield = combatWindow.GetComponent<GraphicalBattlefield>();

    }

	// Update is called once per frame
	void Update ()
    {
        if (overWorld)
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
                        if (players[whoseTurn].equals(castleClicked.Castle.Player))
                        {
                            // TODO when click on your own castle
                            castleClicked.React(getPlayer(whoseTurn));
                            Debug.Log("Leftclicked your own castle");
                            //heroActive = false;
                        }
                    }
                    else
                        prepareDoubleClick = true;
                }
                // Hero is active, either try to make a path to pointed destination, or activate walking towards there.
                else if (heroActive && activeHero.Player.equals(players[whoseTurn]))
                {
                    if (IsWalking())
                    {
                        SetLastStep(true);
                    }
                    // Hero's own position is clicked
                    else if (activeHero.Position.Equals(new Point(posClicked)))
                    {
                        Debug.Log("Clicked on activated hero");
                        // Todo, open hero menu
                    }
                    // If an open square is clicked
                    else if (canWalk[(int)posClicked.x, (int)posClicked.y] != MapMaker.CANNOTWALK)
                    {
                        // Walk to pointer if marked square is clicked by enabling variables that triggers moveHero method on update
                        if (pathMarked && posClicked.Equals(savedClickedPos) && activeHero.CurMovementSpeed > 0)
                        {
                            SetWalking(true);
                            destination = new Point(activeHero.Path[tilesWalking - 1]);
                        }
                        // Activate clicked path
                        else
                        {
                            pathObjects = MarkPath(posClicked);
                        }
                    }
                }
                // activate hero that you clicked on (check after pathing test, to also allow you to walk to that hero)
                else if (reactions[x, y] != null && reactions[x, y].GetType().Name.Equals(typeof(HeroMeetReact)))
                {
                    HeroMeetReact heroClicked = (HeroMeetReact)reactions[x, y];
                    if (players[whoseTurn].equals(heroClicked.Hero.Player))
                    {
                        // TODO when click on your own hero
                        Debug.Log("Leftclicked your own hero");
                        heroActive = true;
                        activeHero = heroClicked.Hero;
                    }
                }
            }

            // Center camera around first hero or castle
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                if (getPlayer(whoseTurn).Heroes[0] != null)
                {
                    cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(activeHero.Position));
                }
                else
                {
                    cameraMovement.centerCamera(getPlayer(whoseTurn).Castle[0].GetPosition().ToVector2());
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
                defendingTest.setUnit(new StoneTroll(), 5,0);
                enterCombat(15,11,activeHero,defendingTest);
            }
            // Upon every update, activedhero will be moved in a direction if walking is enabled
            if (IsWalking())
            {
                Vector2 newPos = PrepareMovement();

                bool stop = false;
                // Test if next tile is occupied by allied hero, checked before movement to new tile is started
                if (newStep)
                {
                    newStep = false;
                    if (activeHero.Path.Count == 1)
                    {
                        int x = (int)activeHero.Path[0].x;
                        int y = (int)activeHero.Path[0].y;

                        if(reactions[x, y] != null)
                        {
                            if(reactions[x, y].GetType().Equals(typeof(DwellingReact)) || reactions[x, y].GetType().Equals(typeof(CastleReact)))
                            {
                                if(reactions[x,y].HasPreReact(activeHero))
                                {
                                    stop = true;
                                }
                            }
                            else
                                stop = true;
                            /*
                            if (reactions[x, y].GetType().Equals(typeof(HeroMeetReact)))
                            {
                                HeroMeetReact hmr = (HeroMeetReact)reactions[x, y];
                                if (hmr.Hero.Player.equals(getPlayer(whoseTurn)))
                                    stop = true;
                            }
                            else if (reactions[x, y].PreReaction != null && reactions[x, y].PreReaction.GetType().Equals(typeof(HeroMeetReact)))
                            {
                                HeroMeetReact hmr = (HeroMeetReact)reactions[x, y].PreReaction;
                                if (hmr.Hero.Player.equals(getPlayer(whoseTurn)))
                                    stop = true;
                            }*/
                        }
                    }
                }

                // If hero has reached a new tile, increment so that he walks towards the next one, reset time animation, and destroy tile object
                if (stop || activeHeroObject.transform.position.Equals(pathObjects[stepNumber].transform.position))
                {
                    Vector2 position = IncrementStep();

                    // Stop the movement when amount of tiles moved has reached the limit, or walking is disabled
                    if (IsLastStep(stepNumber))
                    {
                        Point fromPosition = activeHero.Position;
                        // Set hero position when he stops walking to his isometric position
                        activeHero.Position = HandyMethods.getIsoTilePos(activeHeroObject.transform.position);
                        activeHero.CurMovementSpeed -= stepNumber;

                        int x = destination.x;
                        int y = destination.y;
                        SetWalking(false);
                        SetPathMarked(false);
                        // objectcollision, when final destination is reached
                        if (canWalk[x, y] == MapMaker.TRIGGER)
                        {
                            Debug.Log(reactions[x,y]);
                            bool heroNotDead = true;
                            
                            // If tile is threatened, perform the additional reaction before the main one
                            if (reactions[x, y].HasPreReact(activeHero))
                            {
                                Debug.Log(reactions[x, y].PreReaction);
                                reactions[x, y].PreReact(activeHero);
                                curReaction = reactions[x, y];
                                // Remove hero when false, opponent unit or hero when true
                            }
                            // Only perform the main reaction if the hero didn't die in previous reaction
                            else if (reactions[x, y].React(activeHero))
                            {
                                Debug.Log(reactions[x, y]);
                                //bool react = reactions[x, y].React(activeHero);


                                if (reactions[x, y].GetType().Equals(typeof(ResourceReaction)))
                                {
                                    // TODO visually remove picked up resource
                                }
                                else if (reactions[x, y].GetType().Equals(typeof(ArtifactReaction)))
                                {
                                    // TODO visually remove picked up artifact
                                }
                                else if (reactions[x, y].GetType().Equals(typeof(CastleReact)))
                                {
                                    // TODO change owner of defenseless castle visually
                                    CastleReact cr = (CastleReact) reactions[x, y];
                                    changeCastleOwner(cr);
                                }
                                else if (reactions[x, y].GetType().Equals(typeof(DwellingReact)))
                                {
                                    // TODO visually dwelling has been captured
                                }
                                else if (reactions[x, y].GetType().Equals(typeof(ResourceBuildingReaction)))
                                {
                                    // TODO visually resourceBuilding has been captured
                                }
                            }
                            else
                            {
                                curReaction = reactions[x, y];
                            }
                        }
                        
                        // Clear the previous table reference to current gameobject
                        heroLayer[fromPosition.x, fromPosition.y] = null;
                        // Also move the gameobject's position in the heroLayer table
                        heroLayer[activeHero.Position.x, activeHero.Position.y] = activeHeroObject;
                        
                        // If destination has reaction, set prereact
                        if(reactions[(int)activeHero.Position.x, (int)activeHero.Position.y] != null)
                        {
                            // if you came from a prereact
                            if (!reactions[fromPosition.x, fromPosition.y].GetType().Equals(typeof(HeroMeetReact)))
                            {
                                reactions[activeHero.Position.x, activeHero.Position.y].PreReaction = reactions[fromPosition.x, fromPosition.y].PreReaction;
                            }
                            else
                                reactions[activeHero.Position.x, activeHero.Position.y].PreReaction = reactions[fromPosition.x, fromPosition.y];

                        }
                        // Else, set destination reaction to the heroreaction, and make the tile a triggertile
                        else
                        {

                            // if you came from a prereact
                            if(!reactions[fromPosition.x, fromPosition.y].GetType().Equals(typeof(HeroMeetReact)))
                            {
                                reactions[activeHero.Position.x, activeHero.Position.y] = reactions[fromPosition.x, fromPosition.y].PreReaction;
                            }
                            else
                            {
                                reactions[activeHero.Position.x, activeHero.Position.y] = reactions[fromPosition.x, fromPosition.y];
                            }
                            canWalk[activeHero.Position.x, activeHero.Position.y] = MapMaker.TRIGGER;
                        }

                        // If fromposition didn't have prereact, flip canwalk and remove 
                        if (reactions[(int)fromPosition.x, (int)fromPosition.y].GetType().Equals(typeof(HeroMeetReact)))
                        {
                            canWalk[(int)fromPosition.x, (int)fromPosition.y] = MapMaker.CANWALK;
                            reactions[fromPosition.x, fromPosition.y] = null;
                        }
                        // Else, remove the prereact
                        else
                        {
                            reactions[fromPosition.x, fromPosition.y].PreReaction = null;
                        }
                        
                        // Update herolist and townlist UI, so that onclick listener has updated centercamera
                        updateOverworldUI(players[whoseTurn]);

                    }
                }
                // Execute the movement
                if(!stop)
                {
                    activeHeroObject.transform.position = newPos;
                    cameraMovement.centerCamera(newPos);
                }
            }
            //Nothing is clicked and hero is not walking, listener for change mouse hover
            else
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
                        if (players[whoseTurn].equals(cr.Castle.Player))
                        {
                            // TODO when you hover over your own castle, change mouse pointer
                        }
                    }
                    else if (reactions[x, y].GetType().Equals(typeof(HeroMeetReact)))
                    {
                        HeroMeetReact hmr = (HeroMeetReact)reactions[x, y];
                        if (players[whoseTurn].equals(hmr.Hero.Player))
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
        }
    }

    /// <summary>
    /// Removes visual marker and the Vector2 in activehero's pathtable, and increments stepnumber.
    /// </summary>
    private Vector2 IncrementStep()
    {
        newStep = true;
        Destroy(pathObjects[stepNumber]);
        stepNumber++;
        Vector2 position = activeHero.Path[0];
        activeHero.Path.RemoveAt(0);
        return position;
    }

    /// <summary>
    /// Prepares movement variables, 
    /// Creates a list of positions and creates and returns a list of gameobjects
    /// </summary>
    /// <param name="pos">Destination tile position</param>
    /// <returns>List of instantiated marker objects</returns>
    public List<GameObject> MarkPath(Vector2 pos)
    {
        stepNumber = 0;
        SetPathMarked(true);
        SetLastStep(false);
        savedClickedPos = pos;
        // Needs to clear existing objects if an earlier path was already made
        RemoveMarkers(pathObjects);
        // Call algorithm method that returns a list of Vector2 positions to the point, go through all objects
        activeHero.Path = aStar.calculate(activeHero.Position, new Point(pos));
        DrawPath(activeHero.Path);
        return pathObjects;
    }

    /// <summary>
    /// Graphically draw the path objects
    /// </summary>
    /// <param name="path">list of positions to draw the path</param>
    public void DrawPath(List<Vector2> path)
    {
        // Calculate how many steps the hero will move, if this path is chosen
        int count = tilesWalking = Math.Min(activeHero.Path.Count, activeHero.CurMovementSpeed);
        // For each position, create a gameobject with an image and instantiate it, and add it to a gameobject list for later to be removed

        for (int i = 0; i < activeHero.Path.Count; i++)
        {
            // Create a cloned gameobject of the prefab corresponding to what the marker shall look like
            GameObject pathMarker = new GameObject();
            pathMarker.name = parentToMarkers.name + "(" + path[i].x + ", " + path[i].y + ")";
            pathMarker.transform.parent = parentToMarkers.transform;
            SpriteRenderer sr = pathMarker.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Markers";
            if (i + 1 == activeHero.Path.Count)
            {
                if (i + 1 == tilesWalking)
                    sr.sprite = pathDestYes;
                else
                    sr.sprite = pathDestNo;
            }
            else if (count > 0)
                sr.sprite = pathYes;
            else
                sr.sprite = pathNo;
            count--;
            // set the cloned position to the vector2 object and add it to the list of gameobjects, pathList
            pathMarker.transform.position = HandyMethods.getGraphicPosForIso(path[i]);
            pathObjects.Add(pathMarker);
        }
    }

    /// <summary>
    /// Creates a position with animationspeed and returns it
    /// </summary>
    /// <returns>Position the hero shall be moved to</returns>
    public Vector2 PrepareMovement()
    {
        if(pathObjects != null && stepNumber < pathObjects.Count)
        {
            // Add animation, transform hero position
            return Vector2.MoveTowards(activeHeroObject.transform.position, pathObjects[stepNumber].transform.position, animationSpeed);
        }
        return activeHeroObject.transform.position;
    }

    /// <summary>
    /// Destroy the tile gameobjects and refresh list
    /// </summary>
    /// <param name="li">List that shall be cleared</param>
    public void RemoveMarkers(List<GameObject> li)
    {
        foreach (GameObject go in li)
        {
            Destroy(go);
        }
        li.Clear();
        li = new List<GameObject>();
    }

    public bool IsLastStep(int stepNumber)
    {
        return stepNumber == activeHero.CurMovementSpeed || stepNumber == tilesWalking || lastStep;
    }

    public void SetLastStep(bool w)
    {
        lastStep = w;
    }


    public bool IsWalking()
    {
        return walking;
    }

    public void SetWalking(bool w)
    {
        walking = w;
    }

    public bool IsPathMarked()
    {
        return pathMarked;
    }

    public void SetPathMarked(bool pm)
    {
        pathMarked = pm;
    }

    public Player getPlayer(int index)
    {
        return players[index];
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
	    mapmaker.PlaceBuildings(players);
        
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
    
    /// <summary>
    /// Setting up UI buttons, text and images.
    /// </summary>
    private void GenerateUI()
    {
        overWorldCanvas = GameObject.Find("OverworldCanvas");
        nextRoundBtn = overWorldCanvas.GetComponentInChildren<Button>();

        // Sets UI for first player's heroes and towns
        setOverworldUI(players[whoseTurn]);


        GameObject textObject = GameObject.Find("TextDate");
        dateText = textObject.GetComponent<Text>();
        resourceText = new Text[5];
        for (int i = 0; i < resourceText.Length; i++)
        {
            textObject = GameObject.Find(resourceTextPosition[i]);
            resourceText[i] = textObject.GetComponent<Text>();
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
            resourceText[i].text = getPlayer(whoseTurn).Wallet.GetResource(i) + "";
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
    /// Called by UI click on town
    /// </summary>
    public void EnterTown(Town town)
    {
        Debug.Log(town.Owner.PlayerID);
        if (townWindow.activeSelf)
        {
            overWorldCanvas.SetActive(true);
            townWindow.SetActive(false);
            overWorld = true;
            cameraMovement.enabled = true;
            DestroyGameObjectsInTown();
        }
        else
        {
            overWorldCanvas.SetActive(false);
            DrawTown(town);
            townWindow.SetActive(true);
            overWorld = false;
            cameraMovement.enabled = false;
        }
    }

    public void ExitTown()
    {
        overWorldCanvas.SetActive(true);
        townWindow.SetActive(false);
        overWorld = true;
        cameraMovement.enabled = true;
        DestroyGameObjectsInTown();
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
        buildingsInActiveTown[i].transform.position = placement;
        buildingsInActiveTown[i].transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

        // CONNECTING GAMEOBJECT WITH BUILDING OBJECT: 
        buildingsInActiveTown[i].GetComponent<BuildingOnClick>().Building = town.Buildings[i];
        buildingsInActiveTown[i].GetComponent<BuildingOnClick>().BuildingObjects = buildingsInActiveTown;
        buildingsInActiveTown[i].GetComponent<BuildingOnClick>().Town = town;
        buildingsInActiveTown[i].GetComponent<BuildingOnClick>().Player = getPlayer(whoseTurn);
    }

    /// <summary>
    /// Draws the heroes and units in the town opened.
    /// </summary>
    /// <param name="town">The town from which to get the heroes and towns</param>
    /// <param name="sr">The spriterenderer of the town, used to find positioning of objects</param>
    public void DrawTownArmy(Town town)
    {
        Sprite defaultsprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/NoUnit");
        Vector2 startPosition = new Vector2(townArmyCanvas.transform.position.x * 0.9f, townArmyCanvas.transform.position.y * 1.85f);

        Vector2 topPosition = startPosition;

        // Draws visitinghero with listener for swapping
        int count = 0;
        armyInActiveTown[count] = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
        armyInActiveTown[count].transform.parent = townArmyCanvas.transform;
        armyInActiveTown[count].transform.position = topPosition;
        armyInActiveTown[count].transform.localScale /= 4;
        armyInActiveTown[count].name = "VisitingHero";
        Button visitingHeroButton = armyInActiveTown[count].GetComponent<Button>();
        visitingHeroButton.GetComponent<Image>().sprite = defaultsprite;
        GameObject swapVisitingHero = armyInActiveTown[count];
        visitingHeroButton.onClick.AddListener(() => SwapArmy(swapVisitingHero, town));
        RectTransform rectVisitingHero = armyInActiveTown[count].GetComponent<RectTransform>();
        rectVisitingHero.sizeDelta = new Vector2(armyInActiveTown[count].GetComponent<Image>().sprite.bounds.size.x, armyInActiveTown[count].GetComponent<Image>().sprite.bounds.size.y) * 2;
        count++;
        if (town.VisitingHero != null)
            visitingHeroButton.GetComponent<Image>().sprite = libs.GetPortrait(town.VisitingHero.GetPortraitID());

        // Draws visitingunits with listener for swapping
        for (int i = 0; i < 7; i++)
        {
            topPosition = new Vector2(topPosition.x + (armyInActiveTown[0].GetComponent<Image>().sprite.bounds.size.x / 2), topPosition.y);
            
            armyInActiveTown[count] = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
            armyInActiveTown[count].transform.parent = townArmyCanvas.transform;
            armyInActiveTown[count].transform.position = topPosition;
            armyInActiveTown[count].transform.localScale /= 4;
            armyInActiveTown[count].name = i + "";

            Button visitingUnitButton = armyInActiveTown[count].GetComponent<Button>();
            visitingUnitButton.GetComponent<Image>().sprite = defaultsprite;
            GameObject swapObject = armyInActiveTown[count];
            visitingUnitButton.onClick.AddListener(() => SwapArmy(swapObject, town));
            RectTransform rectVisitingUnit = armyInActiveTown[count].GetComponent<RectTransform>();
            rectVisitingUnit.sizeDelta = new Vector2(armyInActiveTown[count].GetComponent<Image>().sprite.bounds.size.x, armyInActiveTown[count].GetComponent<Image>().sprite.bounds.size.y) * 2;
            stackText[i] = new GameObject();
            stackText[i].transform.parent = townArmyCanvas.transform;
            stackText[i].transform.localScale = townArmyCanvas.transform.localScale;
            stackText[i].transform.localScale /= 2f;
            Text text = stackText[i].AddComponent<Text>();
            text.font = FONT;
            text.fontSize = 25;
            text.text = town.VisitingUnits.getUnitAmount(i) + "";
            text.color = Color.black;
            text.alignment = TextAnchor.LowerRight;
            text.raycastTarget = false;
            stackText[i].transform.position = new Vector2(armyInActiveTown[count].transform.position.x, (armyInActiveTown[count].transform.position.y) - (defaultsprite.bounds.size.y * 0.05f));
            if (town.VisitingUnits.GetUnits()[i] != null)
            {
                stackText[i].SetActive(true);
                visitingUnitButton.GetComponent<Image>().sprite = libs.GetUnit(town.VisitingUnits.GetUnits()[i].GetSpriteID());
            }
            else
                stackText[i].SetActive(false);
            count++;

        }

        Vector2 bottomPosition = startPosition = new Vector2(startPosition.x, startPosition.y - (visitingHeroButton.GetComponent<Image>().sprite.bounds.size.y / 2));

        // Draws stationaryhero with listener for swapping
        armyInActiveTown[count] = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
        armyInActiveTown[count].transform.parent = townArmyCanvas.transform;
        armyInActiveTown[count].transform.position = bottomPosition;
        armyInActiveTown[count].transform.localScale /= 4;
        armyInActiveTown[count].name = "StationaryHero";
        Button stationaryHeroButton = armyInActiveTown[count].GetComponent<Button>();
        stationaryHeroButton.GetComponent<Image>().sprite = defaultsprite;
        GameObject swapStationaryHero = armyInActiveTown[count];
        stationaryHeroButton.onClick.AddListener(() => SwapArmy(swapStationaryHero, town));
        RectTransform rectStationaryHero = armyInActiveTown[count].GetComponent<RectTransform>();
        rectStationaryHero.sizeDelta = new Vector2(armyInActiveTown[count].GetComponent<Image>().sprite.bounds.size.x, armyInActiveTown[count].GetComponent<Image>().sprite.bounds.size.y) * 2;
        count++;
        if (town.StationedHero != null)
            stationaryHeroButton.GetComponent<Image>().sprite = libs.GetPortrait(town.StationedHero.GetPortraitID());

        // Draws stationaryunits with listener for swapping
        for (int i = 0; i < 7; i++)
        {
            bottomPosition = new Vector2(bottomPosition.x + (armyInActiveTown[0].GetComponent<Image>().sprite.bounds.size.x / 2), bottomPosition.y);

            armyInActiveTown[count] = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
            armyInActiveTown[count].transform.parent = townArmyCanvas.transform;
            armyInActiveTown[count].transform.position = bottomPosition;
            armyInActiveTown[count].transform.localScale /= 4;
            armyInActiveTown[count].name = i + 7 + "";
            Button visitingUnitButton = armyInActiveTown[count].GetComponent<Button>();
            visitingUnitButton.GetComponent<Image>().sprite = defaultsprite;
            GameObject swapObject = armyInActiveTown[count];
            visitingUnitButton.onClick.AddListener(() => SwapArmy(swapObject, town));
            RectTransform rectVisitingUnit = armyInActiveTown[count].GetComponent<RectTransform>();
            rectVisitingUnit.sizeDelta = new Vector2(armyInActiveTown[count].GetComponent<Image>().sprite.bounds.size.x, armyInActiveTown[count].GetComponent<Image>().sprite.bounds.size.y) * 2;
            stackText[i + 7] = new GameObject();
            stackText[i + 7].transform.parent = townArmyCanvas.transform;
            stackText[i + 7].transform.localScale = townArmyCanvas.transform.localScale;
            stackText[i + 7].transform.localScale /= 2f;
            Text text = stackText[i + 7].AddComponent<Text>();
            text.font = FONT;
            text.fontSize = 25;
            text.text = town.StationedUnits.getUnitAmount(i) + "";
            text.color = Color.black;
            text.alignment = TextAnchor.LowerRight;
            text.raycastTarget = false;
            stackText[i + 7].transform.position = new Vector2(armyInActiveTown[count].transform.position.x, (armyInActiveTown[count].transform.position.y) - (defaultsprite.bounds.size.y * 0.05f));
            if (town.StationedUnits.GetUnits()[i] != null)
            {
                stackText[i + 7].SetActive(true);
                visitingUnitButton.GetComponent<Image>().sprite = libs.GetUnit(town.StationedUnits.GetUnits()[i].GetSpriteID());
            }
            else
                stackText[i + 7].SetActive(false);
            count++;
        }
    }

    /// <summary>
    /// Redraws the heroes and units in the town opened, called upon when actions in town is performed, and imagebuttons needs to be refreshed.
    /// </summary>
    /// <param name="town">The town from which to get the heroes and towns</param>
    /// <param name="sr">The spriterenderer of the town, used to find positioning of objects</param>
    public void ReDrawArmyInTown(Town town)
    {
        Sprite defaultSprite = UnityEngine.Resources.Load<Sprite>("Sprites/UI/NoUnit");
        int count = 0;

        // Sets hero sprite depending on which hero is in town
        if (town.VisitingHero != null)
            armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = libs.GetPortrait(town.VisitingHero.GetPortraitID());
        else
            armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = defaultSprite;

        
        for (int i=0; i<UnitTree.TREESIZE; i++)
        {
            if (town.VisitingUnits != null && town.VisitingUnits.GetUnits()[i] != null)
            {
                stackText[i].SetActive(true);
                stackText[i].GetComponent<Text>().text = town.VisitingUnits.getUnitAmount(i) + "";
                armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = libs.GetUnit(town.VisitingUnits.GetUnits()[i].GetSpriteID());
            }
            else
            {
                stackText[i].SetActive(false);
                armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = defaultSprite;
            }
        }
        if (town.StationedHero != null)
            armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = libs.GetPortrait(town.StationedHero.GetPortraitID());
        else
            armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = defaultSprite;
        for (int i=0; i< UnitTree.TREESIZE; i++)
        {
            if (town.StationedUnits != null && town.StationedUnits.GetUnits()[i] != null)
            {
                stackText[i + 7].SetActive(true);
                stackText[i + 7].GetComponent<Text>().text = town.StationedUnits.getUnitAmount(i) + "";
                armyInActiveTown[count++].GetComponent<Button>().GetComponent<Image>().sprite = libs.GetUnit(town.StationedUnits.GetUnits()[i].GetSpriteID());
            }
            else
            {
                stackText[i + 7].SetActive(false);
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
                    if (unitPos1 > 6 && unitPos2 > 6)
                    {
                        unitPos1 -= 7;
                        unitPos2 -= 7;
                        town.SwapStationaryUnits(unitPos1, unitPos2);
                    }
                    else if(unitPos1 > 6)
                    {
                        unitPos1 -= 7;
                        town.SwapStationedVisitingUnits(unitPos1, unitPos2);
                    }
                    else if(unitPos2 > 6)
                    {
                        unitPos2 -= 7;
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
                if(((unitpos > 6) && town.StationedUnits != null && town.StationedUnits.GetUnits()[unitpos - 7] != null) || ((unitpos < 7) && town.VisitingUnits != null && town.VisitingUnits.GetUnits()[unitpos] != null))
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
    /// Removes all gameobjects from the gameobject lists of buildings and army
    /// </summary>
    public void DestroyGameObjectsInTown()
    {
        DestroyBuildingsInTown();
        DestroyArmyInTown();
    }

    public void DestroyBuildingsInTown()
    {
        foreach (GameObject building in buildingsInActiveTown)
        {
            if (building != null)
                Destroy(building);
        }
    }

    public void DestroyArmyInTown()
    {
        foreach (GameObject gameObject in armyInActiveTown)
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called by next turn UI button
    /// </summary>
    public void nextTurn()
    {
        // Stop ongoing movement
        if(IsWalking())
        {
            SetLastStep(true);
        }
        else
        {
            // Refresh movementspeed for the upcoming players heroes
            foreach (Hero hero in players[whoseTurn].Heroes)
            {
                if (hero != null)
                    hero.CurMovementSpeed = hero.MovementSpeed;
            }
            // Remove all path markers on the map
            RemoveMarkers(pathObjects);

            // Set active hero and active hero object to the upcoming players first hero

            // On next turn, always set the next player as active player. But if next player 
            // does not exist, keep incrementing till you find an exisiting player.
            do
            {
                incrementTurn();
            }
            while (getPlayer(whoseTurn) == null);


            if(getPlayer(whoseTurn).Heroes[0] != null)
            {
               activeHero = getPlayer(whoseTurn).Heroes[0];
               activeHeroObject = heroLayer[(int)activeHero.Position.x, (int)activeHero.Position.y];
                if (activeHero.Path != null && activeHero.Path.Count > 0)
                {
                    MarkPath(activeHero.Path[activeHero.Path.Count-1]);
                }
               // Center camera to the upcoming players first hero
               cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(activeHero.Position));
            }
            else
            {
                activeHero = null;
                activeHeroObject = null;
                // Center camera to the upcoming players first castle
                cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(getPlayer(whoseTurn).Castle[0].GetPosition().ToVector2()));
            }

            // Gather income for the upcoming player
            getPlayer(whoseTurn).GatherIncome();
            

            // Update wallet UI
            updateResourceText();
            // Update herolist and townlist UI
            updateOverworldUI(players[whoseTurn]);
        }
    }

    /// <summary>
    /// Sets imagebuttons for choosing heroes and entering towns.
    /// </summary>
    /// <param name="player">The player the UI will fetch information from</param>
    private void setOverworldUI(Player player)
    {

        Vector2 nextPosition = new Vector2(overworldInteractablePanelPosition.x + 6.6f, overworldInteractablePanelPosition.y + 5f);
        Vector2 nextTownPosition = new Vector2(nextPosition.x + 1.5f, nextPosition.y);
        // Heroes
        for (int i = 0; i < heroObjects.Length; i++)
        {
            // Initialize all the gameobjects with the buttons
            heroObjects[i] = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
            heroObjects[i].transform.parent = overworldInteractablePanel.transform;
            heroObjects[i].transform.position = nextPosition;
            heroObjects[i].transform.localScale /= 4;
            heroObjects[i].name = player.PlayerID + ", overworldportrait " + i;
            heroObjects[i].SetActive(false);
            heroButton[i] = heroObjects[i].GetComponent<Button>();

            // Sets the button properties if it finds a hero
            if (player.Heroes[i] != null)
            {
                Sprite sprite = libs.GetPortrait(player.Heroes[i].GetPortraitID());

                heroButton[i].GetComponent<Image>().sprite = sprite;
                GameObject swapObject = heroObjects[i];
                Vector2 position = HandyMethods.getGraphicPosForIso(player.Heroes[i].Position.ToVector2());
                Hero hero = player.Heroes[i];
                heroButton[i].onClick.AddListener(() => cameraMovement.centerCamera(position));
                heroButton[i].onClick.AddListener(() => activeHero = hero);
                heroButton[i].onClick.AddListener(() => activeHeroObject = heroLayer[activeHero.Position.x, activeHero.Position.y]);
                RectTransform rectHero = heroObjects[i].GetComponent<RectTransform>();
                rectHero.sizeDelta = new Vector2(heroObjects[i].GetComponent<Image>().sprite.bounds.size.x, heroObjects[i].GetComponent<Image>().sprite.bounds.size.y) * 2;

                nextPosition = new Vector2(nextPosition.x, nextPosition.y - (sprite.bounds.size.y * 0.5f));
                heroObjects[i].SetActive(true);
            }
        }

        // Towns
        for (int i = 0; i < townObjects.Length; i++)
        {
            // Initialize all the gameobjects with the buttons
            townObjects[i] = Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/Button"));
            townObjects[i].transform.parent = overworldInteractablePanel.transform;
            townObjects[i].transform.position = nextTownPosition;
            townObjects[i].transform.localScale /= 70;
            townObjects[i].name = "overworldtownicon " + i;
            townButton[i] = townObjects[i].GetComponent<Button>();

            // Sets the button properties if it finds a castle
            if (player.Castle.Count > i && player.Castle[i] != null)
            {

                Sprite sprite = libs.GetTown(player.Castle[i].Town.GetSpriteID());
                townButton[i].GetComponent<Image>().sprite = sprite;
                Castle castle = player.Castle[i];
                townButton[i].onClick.AddListener(() => EnterTown(castle.Town));
                RectTransform rectTown = townObjects[i].GetComponent<RectTransform>();
                rectTown.sizeDelta = new Vector2(townObjects[i].GetComponent<Image>().sprite.bounds.size.x, townObjects[i].GetComponent<Image>().sprite.bounds.size.y) * 2;
                nextTownPosition = new Vector2(nextTownPosition.x, nextTownPosition.y - (sprite.bounds.size.y * 0.5f));
            }
        }
    }


    /// <summary>
    /// Updates imagebuttons for choosing heroes and entering towns. Called upon by actions on the map, like after walking, after a fight or nextround.
    /// </summary>
    /// <param name="player">The player the UI will fetch information from</param>
    public void updateOverworldUI(Player player)
    {

        // Heroes
        for (int i = 0; i < heroButton.Length; i++)
        {
            // Sets the button properties if it finds a hero
            if (player.Heroes[i] != null)
            {
                heroObjects[i].SetActive(true);
                Sprite sprite = libs.GetPortrait(player.Heroes[i].GetPortraitID());

                heroButton[i] = heroObjects[i].GetComponent<Button>();
                heroButton[i].GetComponent<Image>().sprite = sprite;
                GameObject swapObject = heroObjects[i];
                Vector2 position = HandyMethods.getGraphicPosForIso(player.Heroes[i].Position.ToVector2());
                Hero hero = player.Heroes[i];
                heroButton[i].onClick.RemoveAllListeners();
                heroButton[i].onClick.AddListener(() => cameraMovement.centerCamera(position));
                heroButton[i].onClick.AddListener(() => activeHero = hero);
                heroButton[i].onClick.AddListener(() => activeHeroObject = heroLayer[hero.Position.x, hero.Position.y]);
                RectTransform rectHero = heroObjects[i].GetComponent<RectTransform>();
                rectHero.sizeDelta = new Vector2(heroObjects[i].GetComponent<Image>().sprite.bounds.size.x, heroObjects[i].GetComponent<Image>().sprite.bounds.size.y) * 2;
            }
            // If not, hide the gameobject with the button
            else
                heroObjects[i].SetActive(false);
        }
        
        // Towns
        for (int i=0; i<townButton.Length; i++)
        {
            // Sets the button properties if it finds a castle
            if (player.Castle.Count > i && player.Castle[i] != null)
            {
                townObjects[i].SetActive(true);
                Sprite sprite = libs.GetTown(player.Castle[i].Town.GetSpriteID());

                townButton[i] = townObjects[i].GetComponent<Button>();
                townButton[i].GetComponent<Image>().sprite = sprite;
                Castle castle = player.Castle[i];
                townButton[i].onClick.RemoveAllListeners();
                townButton[i].onClick.AddListener(() => EnterTown(castle.Town));
                RectTransform rectTown = townObjects[i].GetComponent<RectTransform>();
                rectTown.sizeDelta = new Vector2(townObjects[i].GetComponent<Image>().sprite.bounds.size.x, townObjects[i].GetComponent<Image>().sprite.bounds.size.y) * 2;
            }
            // If not, hide the gameobject with the button
            else
                townObjects[i].SetActive(false);
        }
    }

    ///  <summary>
    /// Increment turn, reset turn integer when last player has finished, and increment date
    /// </summary>
    private void incrementTurn()
    {
        if (++whoseTurn >= amountOfPlayers)
        {
            whoseTurn = 0;
            dateText.text = date.incrementDay();
            updateCanBuild();
            getPlayer(whoseTurn).PopulateDwellings();


        }
    }

    /// <summary>
    /// On turn incrementation, the next player's towns hasbuilt is refreshed.
    /// </summary>
    private void updateCanBuild()
    {
        for (int i = 0; i < players.Length; i++)
        {
            foreach(Castle castle in getPlayer(i).Castle)
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
        graphicalBattlefield.beginCombat(width, height, attacker, defender);
        combatWindow.SetActive(true);
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
        cr.Castle.Player = getPlayer(whoseTurn);
        cr.Castle.Town.Owner = getPlayer(whoseTurn);
        getPlayer(whoseTurn).Castle.Add(cr.Castle);
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
        if (reactions[hero.Position.x, hero.Position.y].GetType().Equals((typeof(HeroMeetReact))))
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
}
