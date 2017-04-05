using System;
using System.Collections.Generic;
using MapGenerator;
using UnityEngine;

public class MovementManager
{
    public enum states
    {
        Walking,
        stop
    }

    private states state;
    private Player[] players;
    private Player activePlayer;
    public Hero activeHero;
    AStarAlgo aStar;

    // Values to use with WALK()
    private Reaction[,] reactions;
    private int[,] canWalk;

    // Values to use with marking a path:
    public Sprite pathDestYes;
    public Sprite pathDestNo;
    public Sprite pathYes;
    public Sprite pathNo;
    public List<GameObject> pathObjects;
    public bool pathMarked;
    public int stepNumber;
    public float animationSpeed = 0.1f;
    public bool walking;
    public bool lastStep;
    public int tilesWalking;
    public bool newStep;
    public Point destination;
    private Reaction curReaction;


    private GameManager gameManager;



    public MovementManager(Player[] players, Reaction[,] reactions, int[,] canWalk, AStarAlgo aStar, GameManager gm)
    {
        this.players = players;
        this.aStar = aStar;
        this.gameManager = gm;

        pathDestYes = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerDestYes");
        pathDestNo = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerDestNo");
        pathYes = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerPathYes");
        pathNo = UnityEngine.Resources.Load<Sprite>("Sprites/Pointers/pointerPathNo");
        pathObjects = new List<GameObject>();

        // MAPS:
        this.reactions = reactions;
        this.canWalk = canWalk;
    }

    public void Walk()
    {
        // Getting the new position:
        Vector2 newPos = PrepareMovement();

        bool stop = false;

        // Test if next tile is occupied by allied hero, checked before movement to new tile is started
        if (newStep)
        {
            newStep = false;

            if (activeHero.Path.Count == 1) // IF this is the last step
            {
                int x = (int)activeHero.Path[0].x;
                int y = (int)activeHero.Path[0].y;

                if (reactions[x, y] != null)
                {
                    if (reactions[x, y].GetType().Equals(typeof(DwellingReact)) || reactions[x, y].GetType().Equals(typeof(CastleReact)))
                    {
                        if (reactions[x, y].HasPreReact(activeHero))
                            stop = true;
                    }
                    else
                        stop = true;
                }
            }
        }

        // If hero has reached a new tile, increment so that he walks towards the next one, reset time animation, and destroy tile object
        if (stop || gameManager.activeHeroObject.transform.position.Equals(pathObjects[stepNumber].transform.position))
        {
            Vector2 position = IncrementStep();

            // Stop the movement when amount of tiles moved has reached the limit, or walking is disabled
            if (IsLastStep(stepNumber))
            {
                Point fromPosition = activeHero.Position;

                // Set hero position when he stops walking to his isometric position
                activeHero.Position = HandyMethods.getIsoTilePos(gameManager.activeHeroObject.transform.position);
                activeHero.CurMovementSpeed -= stepNumber;

                int x = destination.x;
                int y = destination.y;
                walking = false;
                pathMarked = false;

                // objectcollision, when final destination is reached
                if (canWalk[x, y] == MapMaker.TRIGGER)
                {
                    Debug.Log(reactions[x, y]);
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
                            CastleReact cr = (CastleReact)reactions[x, y];
                            // TODO: changeCastleOwner(cr);
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
                gameManager.heroLayer[fromPosition.x, fromPosition.y] = null;
                // Also move the gameobject's position in the heroLayer table
                gameManager.heroLayer[activeHero.Position.x, activeHero.Position.y] = gameManager.activeHeroObject;

                // If destination has reaction, set prereact
                if (reactions[activeHero.Position.x, activeHero.Position.y] != null)
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
                    if (!reactions[fromPosition.x, fromPosition.y].GetType().Equals(typeof(HeroMeetReact)))
                    {
                        reactions[activeHero.Position.x, activeHero.Position.y] = reactions[fromPosition.x, fromPosition.y].PreReaction;
                    }
                    else
                    {
                        reactions[activeHero.Position.x, activeHero.Position.y] = reactions[fromPosition.x, fromPosition.y];
                    }
                    canWalk[activeHero.Position.x, activeHero.Position.y] = MapMaker.TRIGGER;
                }

                // If from position didn't have prereact, flip canwalk and remove
                if (reactions[fromPosition.x, fromPosition.y].GetType().Equals(typeof(HeroMeetReact)))
                {
                    canWalk[fromPosition.x, fromPosition.y] = MapMaker.CANWALK;
                    reactions[fromPosition.x, fromPosition.y] = null;
                }
                // Else, remove the prereact
                else
                {
                    reactions[fromPosition.x, fromPosition.y].PreReaction = null;
                }

                // Update herolist and townlist UI, so that onclick listener has updated centercamera
                gameManager.updateOverworldUI(players[gameManager.WhoseTurn]);
            }
        }
        // Execute the movement
        if (!stop)
        {
            gameManager.activeHeroObject.transform.position = newPos;
            gameManager.cameraMovement.centerCamera(newPos);
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
            return Vector2.MoveTowards(gameManager.activeHeroObject.transform.position, pathObjects[stepNumber].transform.position, animationSpeed);
        }
        return gameManager.activeHeroObject.transform.position;
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
        pathMarked = true;
        lastStep = false;
        gameManager.savedClickedPos = pos;
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
            pathMarker.name = gameManager.parentToMarkers.name + "(" + path[i].x + ", " + path[i].y + ")";
            pathMarker.transform.parent = gameManager.parentToMarkers.transform;
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

    /// <summary>
    /// Removes visual marker and the Vector2 in activehero's path table, and increments stepnumber.
    /// </summary>
    private Vector2 IncrementStep()
    {
        newStep = true;
        GameObject.Destroy(pathObjects[stepNumber]);
        stepNumber++;
        Vector2 position = activeHero.Path[0];
        activeHero.Path.RemoveAt(0);
        return position;
    }

    public bool IsLastStep(int stepNumber)
    {
        return stepNumber == activeHero.CurMovementSpeed || stepNumber == tilesWalking || lastStep;
    }

}