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
    [Range(0, 1)]
    public float animationSpeed = 0.1f;


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
    const int DOUBLECLICK_DELAY = 50;
    bool prepareDoubleClick;
    int clickCount;
    public Vector2 savedClickedPos;
    public Town townSelected;

    public Hero activeHero;
    public Hero[] heroes = new Hero[5];

    // Town
    GameObject[] buildingsInActiveTown;
    GameObject[] armyInActiveTown;
    GameObject townArmyCanvas;
    GameObject townWindow;
    public GameObject swapObject;
    public bool overWorld;
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
    public GameObject heroPanel;
    public GameObject heroReactPanel, castleReactPanel, dwellingReactPanel, resourceReactPanel, artifactReactPanel;

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

    public Sprite pathDestYes;
    public Sprite pathDestNo;
    public Sprite pathYes;
    public Sprite pathNo;
    public List<GameObject> pathObjects;

    public static bool ANIMATION_RUNNING;
    private bool pathMarked;


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
        heroPanel = GameObject.Find("HeroPanel");
        heroPanel.SetActive(false);

        heroReactPanel = GameObject.Find("HeroReactPanel");
        heroReactPanel.SetActive(false);
        castleReactPanel = GameObject.Find("CastleReactPanel");
        castleReactPanel.SetActive(false);
        dwellingReactPanel = GameObject.Find("DwellingReactPanel");
        dwellingReactPanel.SetActive(false);
        resourceReactPanel = GameObject.Find("ResourceReactPanel");
        resourceReactPanel.SetActive(false);
        artifactReactPanel = GameObject.Find("ArtifactReactPanel");
        artifactReactPanel.SetActive(false);

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
        movement = new MovementManager(reactions, canWalk, aStar, this);
        pathDestYes = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerDestYes");
        pathDestNo = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerDestNo");
        pathYes = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerPathYes");
        pathNo = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerPathNo");
        pathObjects = new List<GameObject>();
    }

    // Values used in movement:
    private Vector2 nextGraphicalStep;
    private Point nextLogicalStep;

	// Update is called once per frame
    void Update()
    {
        if (overWorld)
        {
            ListenToInputs();

            // Perform animation if the animation is currently running:
            if (ANIMATION_RUNNING)
            {
                Vector2 heroPos = activeHeroObject.transform.position;

                // TODO::: CONTINUE HERE! ANIMATION NOT STOPPING FOR EVERY STEP. NEEDS BETTER EDGECASE!
                // Edgecase:
                if (Vector2.Distance(heroPos, nextGraphicalStep) < 0.1f)
                {
                    // Stopping animation:
                    ANIMATION_RUNNING = false;

                    // Removing the previous pathmarker sprite:
                    Destroy(pathObjects[movement.stepNumber]);
                }
                else
                {
                    // Animating the movement:
                    activeHeroObject.transform.position = AnimateMovementTo(heroPos, nextGraphicalStep);

                    // Camera should be following the player when moving:
                    cameraMovement.centerCamera(heroPos);
                }
            }
            // Taking a new step if there are more steps to take:
            else if ( movement.Activated )
            {
                if (movement.HasNextStep())
                {
                    nextLogicalStep = movement.NextStep();
                    nextGraphicalStep = HandyMethods.getGraphicPosForIso(nextLogicalStep);
                    ANIMATION_RUNNING = true;

                    if (movement.IsLastStep(movement.stepNumber+1))
                    {
                        // Clear the previous table reference to current gameobject
                        heroLayer[movement.StartPosition.x, movement.StartPosition.y] = null;
                        // Also move the gameobject's position in the heroLayer table
                        heroLayer[activeHero.Position.x, activeHero.Position.y] = activeHeroObject;
                    }
                }
                else
                {
                    movement.Activated = false;
                    activeHero.Position = nextLogicalStep;
                    pathMarked = false;
                }
            }
            //Nothing is clicked and hero is not walking, listener for change mouse hover
            else
            {
                ListenToMouseHover();
            }
        }
    }

    private Vector2 AnimateMovementTo( Vector2 from, Vector2 target )
    {
        // Add animation, transform hero position
        return Vector2.MoveTowards( from, target, animationSpeed );
    }

    public void ListenToInputs()
    {
        // if you have clicked once on a castle of possession, give a window of frames to click it again to open castle menu
        if (prepareDoubleClick && ++clickCount == DOUBLECLICK_DELAY)
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

            // The coordinates of the click:
            int x = (int)posClicked.x;
            int y = (int)posClicked.y;
<<<<<<< HEAD

            // Owners castle is clicked
            if (canWalk[x, y] != MapMaker.TRIGGER && reactions[x, y] != null && reactions[x, y].GetType().Equals(typeof(CastleReact)))
=======
            // Fire castle react if you clicked on your own castle
            if (canWalk[(int)posClicked.x, (int)posClicked.y] != MapMaker.TRIGGER && reactions[x, y] != null && reactions[x, y].GetType().Equals(typeof(CastleReact)))
>>>>>>> master
            {
                CastleReact castleClicked = (CastleReact)reactions[x, y];
                if (players[WhoseTurn].equals(castleClicked.Castle.Player))
                    castleClicked.React(players[WhoseTurn]);
            }

            // Hero is active, either try to make a path to pointed destination, or activate walking towards there.
            else if (heroActive && activeHero.Player.equals(players[WhoseTurn]))
            {
                // If click while movement is happening, anywhere else that the hero it self, cancel the movement.
                if (ANIMATION_RUNNING)
                {
                    movement.Deactivate();
                }
                // Hero's own position is clicked
                else if (activeHero.Position.Equals(new Point(posClicked)))
                {
                    SelectHero(activeHero);
                }
                // If an open square is clicked
                else if (canWalk[x, y] != MapMaker.CANNOTWALK)
                {
                    // Second click on a path endpoint, start movement if hero has movementpoints:
                    if (pathMarked && posClicked.Equals(savedClickedPos) && movement.activeHero.CurMovementSpeed > 0)
                    {
                        movement.Activate();
                    }
                    // Calculate a path
                    else
                    {
                        // Needs to clear existing objects if an earlier path was already made
                        RemoveMarkers(pathObjects);

                        // Initializing Movement. Uses new system.
                        movement.PrepareMovement(new Point(posClicked), activeHero);
                        DrawPath(activeHero.Path, movement.totalTilesToBeWalked);
                        savedClickedPos = posClicked;
                        pathMarked = true;
                    }
                }
            }

            // activate hero that you clicked on (check after pathing test, to also allow you to walk to that hero)
            else if (reactions[x, y] != null && reactions[x, y].GetType().Name.Equals(typeof(HeroMeetReact)))
            {
                HeroMeetReact heroClicked = (HeroMeetReact)reactions[x, y];
                if (players[WhoseTurn].equals(heroClicked.Hero.Player))
                {
<<<<<<< HEAD
                    // TODO when click on your own hero
                    Debug.Log("Leftclicked your own hero");
                    heroActive = true;
                    activeHero = heroClicked.Hero;
                    cameraMovement.centerCamera(activeHero.Position.ToVector2());
=======
                    SelectHero(activeHero);
>>>>>>> master
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
        // Escape clicked
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Exit heropanel if it is active
            if (heroPanel.activeSelf)
                ExitHeroPanel();
        }
        // Nextturn by enter
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            nextTurn();
        }
        // Rightclick for combat for testing
        else if (Input.GetMouseButtonDown(1))
        {
            // Fetch the point just clicked and adjust the position in the square to the corresponding isometric position
            Vector2 posClicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            posClicked = HandyMethods.getIsoTilePos(posClicked).ToVector2();

            int x = (int)posClicked.x;
            int y = (int)posClicked.y;

            // Check if any panels are supposed to be opened
            OpenPanelIfReaction(x, y);
            
            // TODO REMOVE
            if (activeHero.Units.GetUnits()[0] == null)
            {
                activeHero.Units.setUnit(new StoneTroll(), 5, 0);
            }
            UnitTree defendingTest = new UnitTree();
            defendingTest.setUnit(new StoneTroll(), 5, 0);
            enterCombat(15, 11, activeHero, defendingTest);
        }
        // Right click up, close the opened panels
        else if(Input.GetMouseButtonUp(1))
        {
            CloseReactPanels();
        }
    }

    /// <summary>
    /// Closes any open reactionPanel
    /// </summary>
    public void CloseReactPanels()
    {
        if(heroReactPanel.activeSelf)
            heroReactPanel.SetActive(false);
        if (castleReactPanel.activeSelf)
            castleReactPanel.SetActive(false);
        if (dwellingReactPanel.activeSelf)
            dwellingReactPanel.SetActive(false);
        if (resourceReactPanel.activeSelf)
            resourceReactPanel.SetActive(false);
        if (artifactReactPanel.activeSelf)
            artifactReactPanel.SetActive(false);
        if (unitPanel.activeSelf)
            unitPanel.SetActive(false);
    }

    /// <summary>
    /// System for opening panels corresponding with reactions
    /// </summary>
    /// <param name="x">x pos</param>
    /// <param name="y">y pos</param>
    public void OpenPanelIfReaction(int x, int y)
    {
        // Open unit card if you right clicked at a unitreaction
        if (reactions[x, y] != null)
        {
            // Unitreaction found, open unitpanel
            if (reactions[x, y].GetType().Equals(typeof(UnitReaction)))
            {
                UnitReaction unitReaction = (UnitReaction)reactions[x, y];
                UnitTree unitTree = unitReaction.Units;
                Unit unit;
                for (int i = 0; i < unitTree.GetUnits().Length; i++)
                {
                    if (unitTree.GetUnits()[i] != null)
                    {
                        unit = unitTree.GetUnits()[i];
                        OpenUnitPanel(null, unit, unitTree.getUnitAmount(i));
                    }
                }
            }
            // Castle react found. If there was a hero prereact at that spot, open heropanel, else open castle panel
            else if (reactions[x, y].GetType().Equals(typeof(CastleReact)))
            {
                CastleReact castleReact = (CastleReact)reactions[x, y];
                Castle castle = castleReact.Castle;
                if (castleReact.HasPreReact() && castleReact.PreReaction.GetType().Equals(typeof(HeroMeetReact)))
                {
                    HeroMeetReact heroMeetReact = (HeroMeetReact)reactions[x, y].PreReaction;
                    OpenHeroReactPanel(heroMeetReact.Hero);
                }
                else
                    OpenCastleReactPanel(castle);
            }
            // HeromeetReact found, open heroPanel
            else if (reactions[x, y].GetType().Equals(typeof(HeroMeetReact)))
            {
                HeroMeetReact heroMeetReact = (HeroMeetReact)reactions[x, y];
                OpenHeroReactPanel(heroMeetReact.Hero);
            }
            // ArtifactReaction found, open ArtifactReactionPanel
            else if (reactions[x, y].GetType().Equals(typeof(ArtifactReaction)))
            {
                ArtifactReaction artifactReaction = (ArtifactReaction)reactions[x, y];
                OpenArtifactReactPanel(artifactReaction.Artifact);
            }
            // ResourceReaction found, open ResourceReactionPanel
            else if (reactions[x, y].GetType().Equals(typeof(ResourceReaction)))
            {
                ResourceReaction resourceReaction = (ResourceReaction)reactions[x, y];
                OpenResourceReactPanel(0); // TODO index
            }
            // DwellingReaction found, open DwellingReact
            else if (reactions[x, y].GetType().Equals(typeof(DwellingReact)))
            {
                DwellingReact dwellingReaction = (DwellingReact)reactions[x, y];
                OpenDwellingReactPanel(dwellingReaction.DwellingBuilding);
            }
        }
    }

    /// <summary>
    /// Activates HeroReactPanel
    /// </summary>
    /// <param name="hero">The Hero</param>
    public void OpenHeroReactPanel(Hero hero)
    {
        // Set hero name, image, and army
        heroReactPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = hero.Name;
        heroReactPanel.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = libs.GetPortrait(hero.GetPortraitID());
        GameObject armyPanel = heroReactPanel.transform.GetChild(2).gameObject;
        for (int i = 0; i < hero.Units.GetUnits().Length; i++)
        {
            GameObject unitObject = armyPanel.transform.GetChild(i).gameObject;
            if (hero.Units.GetUnits()[i] != null)
            {
                unitObject.GetComponent<Image>().sprite = libs.GetUnit(hero.Units.GetUnits()[i].GetSpriteID());
                unitObject.transform.GetChild(0).GetComponent<Text>().text = hero.Units.getUnitAmount(i) + "";
                unitObject.SetActive(true);
            }
            else
            {
                unitObject.transform.GetChild(0).GetComponent<Text>().text = "";
                unitObject.SetActive(true);
            }
        }
        heroReactPanel.SetActive(true);
    }
    
    /// <summary>
    /// Activates CastleReactPanel
    /// </summary>
    /// <param name="castle">The Castle</param>
    public void OpenCastleReactPanel(Castle castle)
    {
        // Set castle name, image, and hero + army
        castleReactPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = castle.Name;
        castleReactPanel.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = libs.GetCastle(castle.GetSpriteID());
        GameObject armyPanel = castleReactPanel.transform.GetChild(2).gameObject;

        GameObject heroObject = armyPanel.transform.GetChild(0).gameObject;
        if(castle.Town.StationedHero != null)
        {
            heroObject.GetComponent<Image>().sprite = libs.GetUnit(castle.Town.StationedHero.GetSpriteID());
            heroObject.SetActive(true);
        }
        else
        {
            heroObject.SetActive(false);
        }

        for (int i = 0; i < castle.Town.StationedUnits.GetUnits().Length; i++)
        {
            GameObject unitObject = armyPanel.transform.GetChild(i+1).gameObject;
            if (castle.Town.StationedUnits.GetUnits()[i] != null)
            {
                unitObject.GetComponent<Image>().sprite = libs.GetUnit(castle.Town.StationedUnits.GetUnits()[i].GetSpriteID());
                unitObject.transform.GetChild(0).GetComponent<Text>().text = castle.Town.StationedUnits.getUnitAmount(i) + "";
                unitObject.SetActive(true);
            }
            else
            {
                unitObject.transform.GetChild(0).GetComponent<Text>().text = "";
                unitObject.SetActive(true);
            }
        }
        castleReactPanel.SetActive(true);
    }
    
    /// <summary>
    /// Activates ResourceReactPanel
    /// </summary>
    /// <param name="resourceIndex">The resourcetype</param>
    public void OpenResourceReactPanel(int resourceIndex)
    {
        resourceReactPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Gold"; // TODO 
        resourceReactPanel.SetActive(true);
    }
    
    /// <summary>
    /// Activates ArtifactReactPanel
    /// </summary>
    /// <param name="artifact">The artifact</param>
    public void OpenArtifactReactPanel(Item artifact)
    {
        artifactReactPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = artifact.SlotType.ToString();
        artifactReactPanel.SetActive(true);
    }
    
    /// <summary>
    /// Activates DwellingReactPanel
    /// </summary>
    /// <param name="dwellingBuilding">The DwellingBuilding</param>
    public void OpenDwellingReactPanel(DwellingBuilding dwellingBuilding)
    {
        resourceReactPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = dwellingBuilding.ToString();
        dwellingReactPanel.SetActive(true);
    }

    /// <summary>
    /// Graphically draw the path objects
    /// </summary>
    /// <param name="path">list of positions to draw the path</param>
    public void DrawPath(List<Vector2> path, int tilesToBeWalked)
    {
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
                if (i + 1 == tilesToBeWalked)
                    sr.sprite = pathDestYes;
                else
                    sr.sprite = pathDestNo;
            }
            else if (tilesToBeWalked > 0)
                sr.sprite = pathYes;
            else
                sr.sprite = pathNo;

            tilesToBeWalked--;

            // set the cloned position to the vector2 object and add it to the list of gameobjects, pathList
            pathMarker.transform.position = HandyMethods.getGraphicPosForIso(path[i]);
            pathObjects.Add(pathMarker);
        }
    }

    /// <summary>
    /// Destroy the tile gameobjects and refresh list
    /// </summary>
    /// <param name="li">List that shall be cleared</param>
    public void RemoveMarkers(List<GameObject> li)
    {
        foreach (GameObject go in li)
        {
            GameObject.Destroy(go);
        }
        li.Clear();
        li = new List<GameObject>();
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
                UnitReaction u = (UnitReaction)reactions[x, y];
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
        if (prepareDoubleClick)
        {
            if (players[WhoseTurn].equals(town.Owner))
            {
                townSelected = null;
                townArmyPanel.SetActive(true);
                DrawTown(town);
                townWindow.SetActive(true);
                overWorld = false;
                cameraMovement.enabled = false;
            }
        }
        else
        {
            prepareDoubleClick = true;
            cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(town.Position.ToVector2()));
        }
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
                heroButton[i].onClick.RemoveAllListeners();
                heroButton[i].onClick.AddListener(() => SelectHero(hero));
                heroObjects[i].SetActive(true);
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
                townButton[i].onClick.RemoveAllListeners();
                townButton[i].onClick.AddListener(() => EnterTown(castle.Town));
                townObjects[i].SetActive(true);
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
        visitingHeroButton.onClick.RemoveAllListeners();
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
            visitingUnitButton.onClick.RemoveAllListeners();
            visitingUnitButton.onClick.AddListener(() => SwapArmy(swapVisitingUnit, town));
            Text text = armyInActiveTown[count].transform.GetChild(0).GetComponent<Text>();
            if (town.VisitingUnits.GetUnits()[i] != null)
            {
                visitingUnitButton.GetComponent<Image>().sprite = libs.GetUnit(town.VisitingUnits.GetUnits()[i].GetSpriteID());
                text.text = town.VisitingUnits.getUnitAmount(i) + "";
                // TODO listener to open unitcard stationedUnitButton.onClick.AddListener(() => );
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
        stationedHeroButton.onClick.RemoveAllListeners();
        stationedHeroButton.onClick.AddListener(() => SwapArmy(swapStationedHero, town));
        if (town.StationedHero != null)
            stationedHeroButton.GetComponent<Image>().sprite = libs.GetPortrait(town.StationedHero.GetPortraitID());
        count++;

        // Draws stationedUnits with listener for swapping
        for (int i = 0; i < UnitTree.TREESIZE; i++)
        {
            armyInActiveTown[count] = townArmyPanel.transform.GetChild(count).gameObject;
            Button stationedUnitButton = armyInActiveTown[count].GetComponent<Button>();
            stationedUnitButton.GetComponent<Image>().sprite = defaultsprite;
            GameObject swapStatonedUnit = armyInActiveTown[count];
            stationedUnitButton.onClick.RemoveAllListeners();
            stationedUnitButton.onClick.AddListener(() => SwapArmy(swapStatonedUnit, town));
            Text text = armyInActiveTown[count].transform.GetChild(0).GetComponent<Text>();
            if (town.VisitingUnits.GetUnits()[i] != null)
            {
                stationedUnitButton.GetComponent<Image>().sprite = libs.GetUnit(town.StationedUnits.GetUnits()[i].GetSpriteID());
                text.text = town.StationedUnits.getUnitAmount(i) + "";
                // TODO listener to open unitcard stationedUnitButton.onClick.AddListener(() => );
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
        if(movement.Activated)
        {
            movement.Activated = false;
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
            RemoveMarkers(pathObjects);

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
               activeHero = players[WhoseTurn].Heroes[0]; // TODO: Last used hero?

               activeHeroObject = heroLayer[activeHero.Position.x, activeHero.Position.y];
                // Setting variables from previous turn:
                if (activeHero.Path != null && activeHero.Path.Count > 0)
                {
                    movement.PrepareMovement(new Point(activeHero.Path[movement.activeHero.Path.Count - 1]), activeHero);
                    DrawPath(movement.activeHero.Path, movement.totalTilesToBeWalked);
                    savedClickedPos = movement.activeHero.Path[movement.activeHero.Path.Count - 1];
                }
                else
                {
                    movement.activeHero = activeHero;
                }
               // Center camera to the upcoming players first hero
               cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(activeHero.Position));
            }
            else
            {
                activeHero = null;
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


    public void OpenUnitPanel(GameObject parent, Unit unit, int amount)
    {
        const int MAX_MOVES = 3;
        // Set unitpanel unittext and unitimage

        unitPanel.transform.parent = parent.transform;
        unitPanel.GetComponent<RectTransform>().sizeDelta = new Vector3(0,0,0);
        string topText = unit.Name;
        // Add amount text if theres a stack
        if (amount > 0)
            topText += " " + amount;
        unitPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = topText;
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

        GameObject abilityPanel = panel.transform.GetChild(2).gameObject;
        
        unitPanel.SetActive(true);
    }

    public void SelectHero(Hero hero)
    {
        if (prepareDoubleClick)
        {
            if (players[WhoseTurn].equals(hero.Player))
            {
                cameraMovement.enabled = false;
                OpenHeroPanel(hero);
            }
        }
        else
        {
            if(players[WhoseTurn].equals(hero.Player))
            {
                // Activate clicked hero
                if(!hero.Equals(activeHero))
                {
                    activeHero = hero;
                    activeHeroObject = heroLayer[activeHero.Position.x, activeHero.Position.y];
                }
                // Center camera and prepare to open heroPanel
                prepareDoubleClick = true;
                cameraMovement.centerCamera(HandyMethods.getGraphicPosForIso(hero.Position.ToVector2()));
            }
        }
    }

    public void OpenHeroPanel(Hero hero)
    {
        heroPanel.SetActive(true);

        GameObject heroContentPanel = heroPanel.transform.GetChild(0).gameObject;
        // Set HeroName Text
        heroPanel.transform.GetChild(1).GetComponent<Text>().text = hero.Name;
        // Set exit button
        GameObject exitButtonObject = heroPanel.transform.GetChild(2).gameObject;
        exitButtonObject.GetComponent<Button>().onClick.RemoveAllListeners();
        exitButtonObject.GetComponent<Button>().onClick.AddListener(() => ExitHeroPanel());

        // Set HeroDescription Text
        heroContentPanel.transform.GetChild(0).GetComponent<Text>().text = hero.Description;
    }

    public void ExitHeroPanel()
    {
        heroPanel.SetActive(false);
        cameraMovement.enabled = true;
    }
}
