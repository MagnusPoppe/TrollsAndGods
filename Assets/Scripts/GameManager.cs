using UnityEngine;
using System.Collections.Generic;
using System;
using Overworld;
using MapGenerator;

public class GameManager : MonoBehaviour 
{

	public MapMaker mapmaker;


	// ONLY SET FOR USE WITH UNITY EDITOR!
	public int widthXHeight = 128;
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
	Reaction[,] reactions;
    IngameObjectLibrary libs;
	AStarAlgo aStar;
    public const float XRESOLUTION = 2598;
	public const float YRESOLUTION = 1299;
	public const float YOFFSET = YRESOLUTION / XRESOLUTION;

    // Generated from mapmaker class:
    Region[] regions;
    int[,] canWalk;

    // Graphical elements
    GameObject[,] groundLayer;
    GameObject[,] buildingLayer;

    // GameManager
    public int amountOfPlayers;
    Player[] players;
    int whoseTurn;
    Date date;


	// Hero listeners and globals:
    bool heroActive;
    GameObject activeHeroObject;
    Hero activeHero;
    GameObject pathDestYes;
    GameObject pathDestNo;
    GameObject pathYes;
    GameObject pathNo;
    List<GameObject> pathObjects;
    private Vector2 curPos;
    private Vector2 toPos;
    bool pathMarked;
    int stepNumber;
    float animationSpeed;
    bool walking;
    bool lastStep;


    // Use this for initialization
    void Start ()
    {
        // Initialize sprite library
        libs = new IngameObjectLibrary();

        // CREATING THE MAP USING MAPMAKER
        GenerateMap();

        activeHeroObject = new GameObject(); // TODO set player1's starthero to activeHero
        
        players = new Player[amountOfPlayers];
        whoseTurn = 0;
        date = new Date();
        curPos = HandyMethods.getIsoTilePos(transform.position);
        pathObjects = new List<GameObject>();
		aStar = new AStarAlgo(canWalk, width, height, false);
    }

	// Update is called once per frame
	void Update ()
    {
		if (Input.GetMouseButtonDown(0))
        {
            // Fetch the point just clicked and adjust the position in the square to the corresponding isometric position
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos = HandyMethods.getIsoTilePos(pos);
            if (heroActive)
            {
                if (IsWalking())
                {
                    SetLastStep(true);
                }
                // Hero's own position is clicked
                else if (curPos.Equals(pos))
                {
                    // Todo, open hero menu
                }
                // If an open square is clicked
                else if (canWalk[(int)pos.x, (int)pos.y] == MapMaker.CANWALK)
                {
                    // Walk to pointer if marked square is clicked by enabling variables that triggers moveHero method on update
                    if (pathMarked && pos.Equals(toPos))
                    {
                        SetWalking(true);
                    }
                    // Activate clicked path
                    else
                    {
                        pathObjects = MarkPath(pos);
                    }
                }
            }
            // TODO else if(clickedOnControlledHero) activate gameobject hero that you clicked on

            // TODO else if clicked on your town, open town layout UI

            // else if(nextTurnClicked) TODO clicked on UI and "next round"
            {
                if (++whoseTurn > amountOfPlayers)
                    whoseTurn = 0;
                activeHero = getPlayer(whoseTurn).Heroes[0];
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            // TODO what happens when you right click things
        }
        // Upon every update, active hero will be moved in a direction if walking is enabled
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
                    curPos = HandyMethods.getIsoTilePos(activeHeroObject.transform.position);
                    SetWalking(false);
                    SetPathMarked(false);
                    RemoveMarkers(pathObjects);
                    // objectcollision, when final destination is reached
                    if (canWalk[(int)curPos.x, (int)curPos.y] == 2)
                    {
                        // todo - reaction
                    }
                }
            }
            // Execute the movement
            activeHeroObject.transform.position = newPos;
        }
    }

    /// <summary>
    /// Prepares movement variables, creates a list of positions and creates and returns a list of gameobjects
    /// </summary>
    /// <param name="pos">Destination tile position</param>
    /// <returns>List of instantiated marker objects</returns>
    public List<GameObject> MarkPath(Vector2 pos)
    {
        stepNumber = 0;
        SetPathMarked(true);
        SetLastStep(false);
        toPos = pos;
        // Needs to clear existing objects if an earlier path was already made
        RemoveMarkers(pathObjects);
        // Call algorithm method that returns a list of Vector2 positions to the point, go through all objects
        List<Vector2> positions = aStar.calculate(curPos, pos);
        // Calculate how many steps the hero will move, if this path is chosen
        int i = activeHero.CurMovementSpeed = Math.Min(positions.Count, activeHero.MovementSpeed);
        // For each position, create a gameobject with an image and instantiate it, and add it to a gameobject list for later to be removed
        foreach (Vector2 no in positions)
        {
            // Create a cloned gameobject of the prefab corresponding to what the marker shall look like
            GameObject pathMarker;
            if (pos.Equals(no) && i > 0)
                pathMarker = pathDestYes;
            else if (pos.Equals(no))
                pathMarker = pathDestNo;
            else if (i > 0)
                pathMarker = pathYes;
            else
                pathMarker = pathNo;
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
            pathMarker = Instantiate(pathMarker);
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
            Destroy(go);
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
			width, height, 40,              // Map Properites TODO: fjern parameter 40/length 
			seed, fillpercentWalkable, smoothIterations,    // BinaryMap Properities
			sites, relaxIterations,                         // Voronoi Properties
			buildingCount
		);
       
		DrawMap(mapmaker.GetMap());

		// SETTING GLOBALS:
		regions = mapmaker.GetRegions();
		canWalk = mapmaker.GetCanWalkMap();


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

            GameObject forest = new GameObject();
            forest.name = "Forests";

            GameObject buildings = new GameObject();
            buildings.name = "Buildings";

            GameObject pickups = new GameObject();
            pickups.name = "Pickups";

        buildingLayer = new GameObject[width, height];

        // DRAWING THE MAP:
        groundLayer = new GameObject[width, height];
		float isometricOffset = 0;
		// Looping through all tile positions:
		for (int y = 0; y < height; y++)
		{

			for (int x = 0; x < width; x++)
			{
                // gets tile value
                int spriteID = map[x, height - 1 - y];

                
                // If ground
                if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Ground)
                {
                    groundLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetGround(spriteID), ground);
                }

                // If building
                else if (libs.GetCategory(spriteID) == IngameObjectLibrary.Category.ResourceBuilding)
                {
                    buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetBuilding(spriteID), buildings);
                }

                // If castle
                else if(libs.GetCategory(spriteID) == IngameObjectLibrary.Category.Castle)
                {
                    buildingLayer[x, y] = placeSprite(x, y, isometricOffset, libs.GetCastle(spriteID), buildings);
                }



				//// TODO: Fjern statiske values
				//if (spriteID >= 0 && spriteID <= 5)
				//{
				//	// TODO: Fjern castle fra verdi "2"
				//	if (spriteID == 2)
				//	{
    //                    buildingLayer[x, y] = new GameObject();
    //                    buildingLayer[x, y].name = "objectsInBuildingLayer (" + x + ", " + y + ")";
    //                    if (y % 2 == 0)
    //                        buildingLayer[x, y].transform.position = new Vector2(x, isometricOffset / 2);
    //                    else
    //                        buildingLayer[x, y].transform.position = new Vector2(x + 0.5f, isometricOffset / 2);

    //                    // make "castle" into "building"
    //                    SpriteRenderer oibl = buildingLayer[x, y].AddComponent<SpriteRenderer>();

    //                    sr.sortingLayerName = "Ground";
    //                    sr.sprite = libs.GetGround(4); // TODO: hardkdoet grass

    //                    spriteID = 6;
    //                    oibl.sortingLayerName = "Buildings";
    //                    oibl.sprite = libs.GetBuilding(spriteID);
				//	}
				//	else
				//	{
				//		// if "ground" or "wall", make "dirt"
				//		if (spriteID == 0 || spriteID == 1)
				//			spriteID = 4;

				//		sr.sortingLayerName = "Ground";
				//		sr.sprite = libs.GetGround(spriteID);    // ny metode
				//	}
				//}

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
}
