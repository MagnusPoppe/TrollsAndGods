using UnityEngine;
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

    public int widthXHeight = 64;
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
    IngameObjectLibrary libs;
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
    Vector2 heroPos;
    Vector2 savedClickedPos;


    // Hero movement
    bool heroActive;
    Hero activeHero;
    GameObject activeHeroObject;
    Sprite pathDestYes;
    Sprite pathDestNo;
    Sprite pathYes;
    Sprite pathNo;
    GameObject parentToMarkers;
    List<GameObject> pathObjects;
    bool pathMarked;
    int stepNumber;
    float animationSpeed;
    bool walking;
    bool lastStep;

    // Town
    GameObject[] buildingsInActiveTown;
    GameObject townWindow;
    bool overWorld;
    Text dateText;
    Text[] resourceText;
    string[] resourceTextPosition = new string[] { "TextGold", "TextWood", "TextOre", "TextCrystal", "TextGem" };


    // Use this for initialization
    void Start ()
    {
        parentToMarkers = new GameObject();
        parentToMarkers.name = "Path";
        width = height = widthXHeight;
        // Initialize sprite library
        libs = new IngameObjectLibrary();

        // CREATING THE MAP USING MAPMAKER
        amountOfPlayers = 5;
        players = new Player[amountOfPlayers];
        GenerateMap();
        reactions = new Reaction[widthXHeight, widthXHeight];

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
            }
        }

        // CREATING THE MAP USING MAPMAKER

        // Creating the camera game object and variables
        GameObject tempCameraObject = GameObject.Find("Main Camera");
        mainCamera = tempCameraObject.GetComponent<Camera>();
        cameraMovement = tempCameraObject.GetComponent<CameraMovement>();
        

        // Set active Hero
        heroActive = true;
        activeHero = getPlayer(0).Heroes[0];
        heroPos = activeHero.Position;
        //activeHeroObject = heroLayer[(int)heroPos.x, height+1-(int)heroPos.y];
        // Initialize turn based and date
        whoseTurn = 0;
        clickCount = 0;
        date = new Date();
        
        //savedClickedPos = HandyMethods.getIsoTilePos(transform.position);
        pathObjects = new List<GameObject>();

		    aStar = new AStarAlgo(canWalk, width, height, false);
        townWindow = GameObject.Find("Town");
        townWindow.SetActive(false);
        overWorld = true;

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
            if (Input.GetMouseButtonDown(0))
            {
                // Fetch the point just clicked and adjust the position in the square to the corresponding isometric position
                Vector2 posClicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                posClicked = HandyMethods.getIsoTilePos(posClicked);
                int x = (int)posClicked.x;
                int y = (int)posClicked.y;

                // Owners castle is clicked
                if (reactions[x, y] != null && reactions[x, y].GetType().Name.Equals(typeof(CastleReact)))
                {
                    if (prepareDoubleClick)
                    {
                        CastleReact castleClicked = (CastleReact)reactions[x, y];
                        if (players[whoseTurn].equals(castleClicked.Castle.Player))
                        {
                            // TODO when click on your own castle
                            //EnterTown();
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
                    else if (heroPos.Equals(posClicked))
                    {
                        // Todo, open hero menu
                    }
                    // If an open square is clicked
                    else if (canWalk[(int)posClicked.x, (int)posClicked.y] == MapMaker.CANWALK)
                    {
                        // Walk to pointer if marked square is clicked by enabling variables that triggers moveHero method on update
                        if (pathMarked && posClicked.Equals(savedClickedPos))
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
            // TODO right mousebutton clicked
            else if (Input.GetMouseButtonDown(1))
            {
                if (IsWalking())
                {
                    SetLastStep(true);
                }
                // TODO: temp town creation
                VikingTown t = new VikingTown(new Player(0,0));
                for (int i = 0; i < t.Buildings.Length; i++)
                {
                    t.Buildings[i].Build();
                }
                EnterTown(t);
            }
            // Upon every update, activedhero will be moved in a direction if walking is enabled
            if (IsWalking())
            {
                Vector2 newPos = PrepareMovement();

                // If hero has reached a new tile, increment so that he walks towards the next one, reset time animation, and destroy tile object
                if (activeHeroObject.transform.position.Equals(pathObjects[stepNumber].transform.position))
                {
                    Destroy(pathObjects[stepNumber]);
                    stepNumber++;
                    animationSpeed = 0f;
                    // Stop the movement when amount of tiles moved has reached the limit, or walking is disabled
                    if (IsLastStep())
                    {
                        // Set hero position when he stops walking to his isometric position
                        heroPos = HandyMethods.getIsoTilePos(activeHeroObject.transform.position);
                        int x = (int)heroPos.x;
                        int y = (int)heroPos.y;
                        SetWalking(false);
                        SetPathMarked(false);
                        RemoveMarkers(pathObjects);
                        // objectcollision, when final destination is reached
                        if (canWalk[x, y] == 2)
                        {
                            if (reactions[x, y].React(activeHero))
                            {
                                if (reactions[x, y].GetType().Name.Equals(typeof(HeroMeetReact)))
                                {
                                    // TODO if battle, remove hero that is now set to null
                                }
                                else if (reactions[x, y].GetType().Name.Equals(typeof(UnitReaction)))
                                {
                                    // TODO remove either hero or unit
                                }
                                else if (reactions[x, y].GetType().Name.Equals(typeof(ResourceReaction)))
                                {
                                    // TODO remove picked up resource
                                }
                                else if (reactions[x, y].GetType().Name.Equals(typeof(ArtifactReaction)))
                                {
                                    // TODO remove picked up artifact
                                }
                                else if (reactions[x, y].GetType().Name.Equals(typeof(CastleReact)))
                                {
                                    // TODO enemy town was attacked

                                }
                                else if (reactions[x, y].GetType().Name.Equals(typeof(DwellingReact)))
                                {
                                    // TODO dweeling has been captured
                                }
                            }
                        }
                        // TODO when hero moves, set origin tile's canWalk 0 or 2 whether theres an reaction there. Also set destination tile's canWalk to 2
                    }
                }
                // Execute the movement
                activeHeroObject.transform.position = newPos;
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
                    if (reactions[x, y].GetType().Name.Equals(typeof(CastleReact)))
                    {
                        CastleReact cr = (CastleReact)reactions[x, y];
                        if (players[whoseTurn].equals(cr.Castle.Player))
                        {
                            Debug.Log(x + " - " + y + " CastleReact Dummy");
                            // TODO when you hover over your own castle, change mouse pointer
                        }
                    }
                    else if (reactions[x, y].GetType().Name.Equals(typeof(HeroMeetReact)))
                    {
                        Debug.Log(x + " - " + y + " HeroMeetReact Dummy");
                        // TODO when you hover over an hero, change mouse pointer
                    }
                    else if (reactions[x, y].GetType().Name.Equals(typeof(UnitReaction)))
                    {
                        Debug.Log(x + " - " + y + " UnitReaction Dummy");
                        // TODO when you hover over an neutral unit, change mouse pointer
                    }
                }
            }
        }
    }

    /// <summary>
    /// Prepares movement variables, 
    /// ¨Åcreates a list of positions and creates and returns a list of gameobjects
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
        List<Vector2> positions = aStar.calculate(heroPos, pos);
        // Calculate how many steps the hero will move, if this path is chosen
        int i = activeHero.CurMovementSpeed = Math.Min(positions.Count, activeHero.MovementSpeed);
        // For each position, create a gameobject with an image and instantiate it, and add it to a gameobject list for later to be removed

        foreach (Vector2 no in positions)
        {
            // Create a cloned gameobject of the prefab corresponding to what the marker shall look like
            GameObject pathMarker = new GameObject();
            pathMarker.name = parentToMarkers.name + "(" + no.x + ", " + no.y + ")";
            pathMarker.transform.parent = parentToMarkers.transform;
            SpriteRenderer sr = pathMarker.AddComponent<SpriteRenderer>();
            sr.sortingLayerName = "Markers";
            if (pos.Equals(no) && i > 0)
                sr.sprite = pathDestYes;
            else if (pos.Equals(no))
                sr.sprite = pathDestNo;
            else if (i > 0)
                sr.sprite = pathYes;
            else
                sr.sprite = pathNo;
            i--;
            Vector2 modified;
            if (no.y % 2 == 0)
            {
                modified = new Vector2(no.x, no.y / 2 / 2);
            }
            else
            {
                modified = new Vector2(no.x + 0.5f, no.y / 2 / 2);
            }
            // set the cloned position to the vector2 object, instantiate it and add it to the list of gameobjects, pathList
            pathMarker.transform.position = modified;
            pathObjects.Add(pathMarker);
        }
        return pathObjects;
    }



    /// <summary>
    /// Creates a position with animationspeed and returns it
    /// </summary>
    /// <returns>Position the hero shall be moved to</returns>
    public Vector2 PrepareMovement()
    {
        // Add animation, transform hero position
        animationSpeed += Time.deltaTime;
        return Vector2.Lerp(transform.position, pathObjects[stepNumber].transform.position, animationSpeed);
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
    }

    public bool IsLastStep()
    {
        return stepNumber == activeHero.CurMovementSpeed || lastStep;
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
		width = widthXHeight;
		height = widthXHeight;

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
                
				//int spriteID = map[x, height - 1 - y];
				int spriteID = map[x, y];
				// TODO: CURRENT: Denne er snudd

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
					Debug.Log("ResourceBuilding: @(" + x + "," + y + ")");

					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDwelling(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(MapMaker.GRASS_SPRITEID), ground); //TODO:temp
				}

				// If resource buildings
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.ResourceBuildings)
				{
					Debug.Log("ResourceBuilding: @(" + x + "," + y + ")");
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
			isometricOffset += YOFFSET; // 0.57747603833865814696485623003195f;
		}
	}

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
					Debug.Log("ResourceBuilding: @(" + x + "," + y + ")");

					buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDwelling(spriteID), buildings);
					groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetDebugSprite(canWalk[x,y]), DebugTiles); //TODO:temp
				}

				// If resource buildings
				else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.ResourceBuildings)
				{
					Debug.Log("ResourceBuilding: @(" + x + "," + y + ")");
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
			isometricOffset += YOFFSET; // 0.57747603833865814696485623003195f;
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
            townWindow.SetActive(false);
            overWorld = true;
            cameraMovement.enabled = true;
            DestroyBuildingsInTown();
        }
        else
        {
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
                buildingsInActiveTown[i] = new GameObject();
                buildingsInActiveTown[i].name = town.Buildings[i].Name;
                buildingsInActiveTown[i].transform.position = placement;
                buildingsInActiveTown[i].transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);
                buildingsInActiveTown[i].transform.parent = townWindow.transform;


                // TODO: bruk prefab for bygning med prekonstruert PolygonCollider2D
                PolygonCollider2D collider = buildingsInActiveTown[i].AddComponent<PolygonCollider2D>();
                collider.isTrigger = true;


                // Adds a sprite rendered to display the building
                SpriteRenderer buildingSr = buildingsInActiveTown[i].AddComponent<SpriteRenderer>();
                buildingSr.sprite = libs.GetTown(town.Buildings[i].GetSpriteID());
                buildingSr.sortingLayerName = "TownBuildings";
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
        if (++whoseTurn > amountOfPlayers)
        {
            whoseTurn = 0;
            dateText.text = date.incrementDay();
        }
        //activeHero = getPlayer(whoseTurn).Heroes[0]; // TODO UNCOMMENT
        //getPlayer(whoseTurn).GatherIncome(); // TODO UNCOMMENT
    }

    private Vector2 RealLogicalPostition(Vector2 position)
    {
        return new Vector2(position.x, height + 1 - position.y);
    }
}
