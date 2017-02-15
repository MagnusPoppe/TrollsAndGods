using UnityEngine;
using System.Collections.Generic;
using System;
using Overworld;
using MapGenerator;

public class GameManager : MonoBehaviour {

    Map m;
    int[,] canWalk;
    public int amountOfPlayers;
    Player[] players;
    int whoseTurn;
    Date date;
    Reaction[,] reactions;
    AStarAlgo aStar;

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
        activeHeroObject = new GameObject(); // TODO set player1's starthero to activeHero
        NewTestHero(); // TEST active hero
        m = new Map();
        canWalk = m.mapmaker.GetCanWalkMap();
        players = new Player[amountOfPlayers];
        whoseTurn = 0;
        date = new Date();
        curPos = HandyMethods.getIsoTilePos(transform.position);
        pathObjects = new List<GameObject>();
        aStar = new AStarAlgo(canWalk, m.GetWidthOfMap(), m.GetHeightOfMap(), false);
    }

    void NewTestHero()
    {
        activeHeroObject.transform.position = new Vector2(5, 5);
        Sprite sp = UnityEngine.Resources.Load<Sprite>("Sprites/Buildings/Castle");
        SpriteRenderer spr = activeHeroObject.AddComponent<SpriteRenderer>();
        spr.sprite = sp;
        Instantiate(activeHeroObject);
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


}
