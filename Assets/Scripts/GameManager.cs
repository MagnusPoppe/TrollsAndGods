using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using TownView;
using MapGenerator;

public class GameManager : MonoBehaviour
{

    public MapMaker mapmaker;

    public Sprite[] groundTiles;

    // Loads in camera variables
    Camera mainCamera;
    CameraMovement cameraMovement;

	// ONLY SET FOR USE WITH UNITY EDITOR!
	public bool CanWalkDebugMode = false;

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
    const int CLICKSPEED = 20;
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

    // Town
    GameObject[] buildingsInActiveTown;
    GameObject townWindow;
    bool overWorld;

    // UI
    Button nextRoundBtn;
    Text dateText;
    Text[] resourceText;
    string[] resourceTextPosition = new string[] { "TextGold", "TextWood", "TextOre", "TextCrystal", "TextGem" };
    GameObject overWorldCanvas;


    // Use this for initialization
    void Start ()
    {
        parentToMarkers = new GameObject();
        parentToMarkers.name = "Path";
        width = WIDTH;
        height = HEIGHT;
        // Initialize sprite library
        libs = new IngameObjectLibrary();

        // CREATING THE MAP USING MAPMAKER
        amountOfPlayers = 5;
        players = new Player[amountOfPlayers];
        // CREATING THE MAP USING MAPMAKER
        GenerateMap();
        reactions = new Reaction[width, height];

        pathDestYes = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerDestYes");
        pathDestNo = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerDestNo");
        pathYes = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerPathYes");
        pathNo = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerPathNo");

        // Add reactions to buildings in regions
        foreach (Region r in regions)
        {
            if (r.GetType().Equals(typeof(LandRegion)))
            {
                LandRegion lr = (LandRegion)r;
                lr.makeReactions(reactions);

                // Add reactions to castles and heroes
                /*if (lr.GetCastle() != null)
                {
                    reactions[(int)lr.GetCastle().GetPosition().x, (int)lr.GetCastle().GetPosition().y] = new CastleReact(lr.GetCastle(), lr.GetCastle().GetPosition());
                }*/
                if (lr.GetHero() != null)
                    reactions[(int)lr.GetHero().Position.x, (int)lr.GetHero().Position.y] = new HeroMeetReact(lr.GetHero(), lr.GetHero().Position);
            }
        }

        // Creating the camera game object and variables
        GameObject tempCameraObject = GameObject.Find("Main Camera");
        mainCamera = tempCameraObject.GetComponent<Camera>();
        cameraMovement = tempCameraObject.GetComponent<CameraMovement>();
        

        // Set active Hero
        heroActive = true;
        activeHero = getPlayer(0).Heroes[0];
        activeHeroObject = heroLayer[(int)activeHero.Position.x, (int)activeHero.Position.y];
        
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
                posClicked = HandyMethods.getIsoTilePos(posClicked);

                int x = (int)posClicked.x;
                int y = (int)posClicked.y;
                // Owners castle is clicked
                if (reactions[x, y] != null && reactions[x, y].GetType().Equals(typeof(CastleReact)))
                {
                    if (prepareDoubleClick)
                    {
                        CastleReact castleClicked = (CastleReact)reactions[x, y];
                        if (players[whoseTurn].equals(castleClicked.Castle.Player))
                        {
                            // TODO when click on your own castle
                            castleClicked.React(getPlayer(whoseTurn));
                            Debug.Log("Leftclicked your own castle");
                            heroActive = false;
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
                    else if (activeHero.Position.Equals(posClicked))
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

                // TODO else if(GUInextTurnClicked)
                //else if (false)
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
                    cameraMovement.centerCamera(getPlayer(whoseTurn).Castle[0].GetPosition());
                }
            }
            // Nextturn by enter
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                nextTurn();
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
                        if (reactions[x, y] != null && (reactions[x, y].GetType().Equals(typeof(HeroMeetReact)) || (reactions[x, y].PreReaction != null && reactions[x, y].PreReaction.GetType().Equals(typeof(HeroMeetReact)))))
                        {
                            HeroMeetReact hmr = (HeroMeetReact)reactions[x, y].PreReaction;
                            // If the upcoming tile has a trigger with an allied hero in it, finish his movement
                            if (hmr == null || hmr.Hero.Player.equals(getPlayer(whoseTurn)))
                            {
                                stop = true;
                            }
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
                        Vector2 fromPosition = activeHero.Position;
                        // Set hero position when he stops walking to his isometric position
                        activeHero.Position = HandyMethods.getIsoTilePos(activeHeroObject.transform.position);
                        activeHero.CurMovementSpeed -= stepNumber;

                        int x = (int)activeHero.Position.x;
                        int y = (int)activeHero.Position.y;
                        SetWalking(false);
                        SetPathMarked(false);
                        // objectcollision, when final destination is reached
                        if (canWalk[x, y] == MapMaker.TRIGGER)
                        {
                            bool heroNotDead = true;
                            
                            // If tile is threatened, perform the additional reaction before the main one
                            if (reactions[x, y].HasPreReact(activeHero))
                            {
                                heroNotDead = reactions[x, y].PreReact(activeHero);
                                // Remove hero when false, opponent unit or hero when true
                            }
                            // Only perform the main reaction if the hero didn't die in previous reaction
                            if (heroNotDead)
                            {
                                bool react = reactions[x, y].React(activeHero);

                                if (reactions[x, y].GetType().Equals(typeof(HeroMeetReact)))
                                {
                                    // TODO if battle, visually remove the hero that is now set to null (true when attacker won)
                                }
                                else if (reactions[x, y].GetType().Equals(typeof(UnitReaction)))
                                {
                                    // TODO visually remove either hero or unit (true when attacker won)
                                }
                                else if (reactions[x, y].GetType().Equals(typeof(ResourceReaction)))
                                {
                                    // TODO visually remove picked up resource
                                }
                                else if (reactions[x, y].GetType().Equals(typeof(ArtifactReaction)))
                                {
                                    // TODO visually remove picked up artifact
                                }
                                else if (reactions[x, y].GetType().Equals(typeof(CastleReact)))
                                {
                                    // TODO visually remove picked up artifact
                                }
                                else if (reactions[x, y].GetType().Equals(typeof(DwellingReact)))
                                {
                                    // TODO visually dweeling has been captured
                                }
                                else if (reactions[x, y].GetType().Equals(typeof(ResourceBuildingReaction)))
                                {
                                    // TODO visually resourceBuilding has been captured
                                }
                            }
                            else
                            {

                            }
                        }
                        
                        // Clear the previous table reference to current gameobject
                        heroLayer[(int)fromPosition.x, (int)fromPosition.y] = null;
                        // Also move the gameobject's position in the heroLayer table
                        heroLayer[(int)activeHero.Position.x, (int)activeHero.Position.y] = activeHeroObject;
                        
                        /*
                        // Set origin tile's canWalk 0 or 2 if no reaction there
                        if (reactions[(int)fromPosition.x, (int)fromPosition.y] == null)
                            canWalk[(int)fromPosition.x, (int)fromPosition.y] = MapMaker.CANWALK;
                        // Set destination tile's canWalk to 2
                        canWalk[(int)activeHero.Position.x, (int)activeHero.Position.y] = MapMaker.TRIGGER;
                        // TODO also set reactions frompos and topos
                        */

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
                mousePos = HandyMethods.getIsoTilePos(mousePos);
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
        activeHero.Path = aStar.calculate(activeHero.Position, pos);
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
			buildingCount
		);

        int[,] map = mapmaker.GetMap();

		// SETTING GLOBALS:
		regions = mapmaker.GetRegions();
		canWalk = mapmaker.GetCanWalkMap();

        // SETTING UP REGIONS WITH PLAYERS, CASTLE AND HERO:
        mapmaker.initializePlayers(map, canWalk, players);


        if (CanWalkDebugMode)
		{
			DrawDebugMap(map, canWalk);
		}
		else
		{
			DrawMap(map);
		}

        // Kaster mapmaker
        mapmaker = null;
	}
    
    private void GenerateUI()
    {
        overWorldCanvas = GameObject.Find("OverworldCanvas");
        nextRoundBtn = overWorldCanvas.GetComponentInChildren<Button>();


        GameObject textObject = GameObject.Find("TextDate");
        dateText = textObject.GetComponent<Text>();
        resourceText = new Text[5];
        for (int i = 0; i < resourceText.Length; i++)
        {
            textObject = GameObject.Find(resourceTextPosition[i]);
            resourceText[i] = textObject.GetComponent<Text>();
            resourceText[i].text = i + ""; // TODO currentPlayer.getResource(i);
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
				if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Ground)
				{
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(spriteID), ground);
				}

				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Environment)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetEnvironment(spriteID), mountains);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(MapMaker.GRASS_SPRITEID), ground); //TODO:temp
				}

				// If dwelling
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Dwellings)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDwelling(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(MapMaker.GRASS_SPRITEID), ground); //TODO:temp
				}

				// If resource buildings
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.ResourceBuildings)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetResourceBuilding(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(MapMaker.GRASS_SPRITEID), ground); //TODO:temp
				}

				// If hero
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Heroes)
				{
					heroLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetHero(spriteID), heroes);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(IngameObjectLibrary.GROUND_START), ground);
				}

				// If castle
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Castle)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetCastle(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(MapMaker.GRASS_SPRITEID), ground); //TODO:temp
				}

				// If debug mode:
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Debug)
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

				if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Environment)
				{
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x, y]), DebugTiles); //TODO:temp
				}

				// If dwelling
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Dwellings)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDwelling(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x,y]), DebugTiles); //TODO:temp
				}

				// If resource buildings
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.ResourceBuildings)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetResourceBuilding(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x, y]), DebugTiles); //TODO:temp
				}

				// If hero
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Heroes)
				{
					heroLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetHero(spriteID), heroes);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x, y]), DebugTiles); //TODO:temp
				}

				// If castle
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Castle)
				{
					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetCastle(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x, y]), DebugTiles); //TODO:temp
				}

				// If debug mode:
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Ground)
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
        if (townWindow.activeSelf)
        {
            overWorldCanvas.SetActive(true);
            townWindow.SetActive(false);
            overWorld = true;
            cameraMovement.enabled = true;
            DestroyBuildingsInTown();
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

        // loads in the town buildings
        for (int i = 0; i < town.Buildings.Length; i++)
        {

            // If the building is built, draw it 
            if (town.Buildings[i].Built)
            {

                // Gets parent X,Y and uses offset coords to draw in place
                Vector2 placement = new Vector2(
                    townWindow.transform.position.x + town.Buildings[i].Placement.x,
                    townWindow.transform.position.y + town.Buildings[i].Placement.y
                );

                // Creates a game object for the building, gives it a name and places and scales it properly
                string prefabPath = "Prefabs/" + town.Buildings[i].Name;
                buildingsInActiveTown[i] = Instantiate(UnityEngine.Resources.Load<GameObject>(prefabPath));
                buildingsInActiveTown[i].transform.position = placement;
                buildingsInActiveTown[i].transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

                // CONNECTING GAMEOBJECT WITH BUILDING OBJECT: 
                Debug.Log(town.Buildings[i].ToString());
                buildingsInActiveTown[i].GetComponent<BuildingOnClick>().Building = town.Buildings[i];
                buildingsInActiveTown[i].GetComponent<BuildingOnClick>().BuildingObjects = buildingsInActiveTown;

            }
        }

    }

    public void DestroyBuildingsInTown()
    {
        foreach (GameObject building in buildingsInActiveTown)
        {
            if (building != null)
                Destroy(building);
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
               if (activeHero.Path != null)
                    DrawPath(activeHero.Path);
               // Center camera to the upcoming players first hero
               cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(activeHero.Position));
            }
            else
            {
                activeHero = null;
                activeHeroObject = null;
                // Center camera to the upcoming players first castle
                cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(getPlayer(whoseTurn).Castle[0].GetPosition()));
            }
            // Gathert income for the upcoming player
            getPlayer(whoseTurn).GatherIncome();
        }
    }

    // Increment turn, reset turn integer when last player has finished, and increment date
    private void incrementTurn()
    {
        if (++whoseTurn >= amountOfPlayers)
        {
            whoseTurn = 0;
            dateText.text = date.incrementDay();
        }
    }

}
