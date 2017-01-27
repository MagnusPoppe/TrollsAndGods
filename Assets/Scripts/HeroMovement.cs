using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Script for overworld movement and interaction with objects
/// </summary>
public class HeroMovement : MonoBehaviour
{
    GenerateMap gm;
    GameObject g;
    public GameObject pathDestYes;
    public GameObject pathDestNo;
    public GameObject pathYes;
    public GameObject pathNo;
    Vector2 curPos;
    Vector2 toPos;
    List<GameObject> pathObjects;
    List<Vector2> positions;
    bool pathMarked;
    int stepNumber;
    float animationSpeed;
    public int heroSpeed;
    public int curSpeed;
    private bool walking;
    private bool lastStep;
    private AStarAlgo aStar;

    /// <summary>
    /// Upon creation, set current position and a reference to the generated map object
    /// </summary>
    void Start ()
    {
        heroSpeed = 8; // todo
        curPos = transform.position;
        pathObjects = new List<GameObject>();
        g = GameObject.Find("MapGenerator");
        gm = g.GetComponent<GenerateMap>();
        aStar = new AStarAlgo(gm.canWalk, gm.GetWidth(), gm.GetHeight(), false);
    }
	
    /// <summary>
    /// Every frame checks if you clicked on a location in the map or if the hero is walking.
    /// </summary>
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Fetch the point just clicked and adjust the position in the square to the middle of it
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.x = (int)(pos.x + 0.5);
            pos.y = (int)(pos.y + 0.5);
            // When mousebutton is clicked an already ongoing movement shall be stopped
            if (isWalking())
            {
                setLastStep(true);
            }
            // Hero's own position is clicked
            else if (curPos.Equals(pos))
            {
                // Todo, open hero menu
            }
            // If an open square is clicked
            else if (gm.isNotBlocked((int)pos.x, (int)pos.y))
            {
                // Walk to pointer if marked square is clicked by enabling variables that triggers moveHero method on update
                if (pathMarked && pos.Equals(toPos))
                {
                    setWalking(true);
                }
                // Activate clicked path
                else
                {
                    pathObjects = markPath(pos);
                }
            }
        }
        else if(Input.GetKeyDown("space"))
        {
            if (isWalking())
            {
                setLastStep(true);
            }
            else if (pathMarked)
            {
                setWalking(true);
            }
        }
        // Upon every update, hero will be moved in a direction if walking is enabled
        if (isWalking())
        {
            transform.position = moveHero();
        }
    }

    /// <summary>
    /// Prepares movement variables, creates a list of positions and creates and returns a list of gameobjects
    /// </summary>
    /// <param name="pos">Destination tile position</param>
    /// <returns>List of instantiated marker objects</returns>
    private List<GameObject> markPath(Vector2 pos)
    {
        pathMarked = true;
        stepNumber = 0;
        animationSpeed = 0f;
        setLastStep(false);
        toPos = pos;
        // Call algorithm method that returns a list of Vector2 positions to the point, go through all objects
        positions = aStar.calculate(curPos, pos);
        // Calculate how many steps the hero can move
        curSpeed = Math.Min(positions.Count, heroSpeed);
        int i = curSpeed;
        // For each position, create a gameobject with an image and instantiate it, and add it to a gameobject list for later to be removed
        foreach (Vector2 no in positions)
        {
            // Create a cloned gameobject of the prefab corresponding to what the marker shall look like
            GameObject pathMarker;
            if (pos == no && i > 0)
                pathMarker = pathDestYes;
            else if (pos == no)
                pathMarker = pathDestNo;
            else if (i > 0)
                pathMarker = pathYes;
            else
                pathMarker = pathNo;
            i--;
            // set the cloned position to the vector2 object, instantiate it and add it to the list of gameobjects, pathList
            pathMarker.transform.position = no;
            pathMarker = Instantiate(pathMarker);
            pathObjects.Add(pathMarker);
        }
        return pathObjects;
    }

    /// <summary>
    /// Creates a position with animationspeed and returns it, also checks if hero has reached new tile and if the movement shall be stopped
    /// </summary>
    /// <returns>Position the hero shall be moved to</returns>
    private Vector2 moveHero()
    {
        // Add animation, transform hero position
        animationSpeed += Time.deltaTime; 
        Vector2 pos = Vector2.Lerp(transform.position, positions[stepNumber], animationSpeed);

        // If hero reaches a new tile, increment so that he walks towards the next one, reset time animation, and destroy tile object
        if (((Vector2)transform.position).Equals((positions[stepNumber])))
        {
            // Destroy the tile object he has reached
            Destroy(pathObjects[stepNumber]);
            stepNumber++;
            animationSpeed = 0f;
            // Stop the movement when amount of tiles moved has reached speed, or walking is disabled
            if (stepNumber == curSpeed || isLastStep())
            {
                // Set hero position variable, and refresh toposition
                curPos = transform.position;
                setWalking(false);
                pathMarked = false;
                // Destroy the tile gameobjects
                foreach (GameObject go in pathObjects)
                    Destroy(go);

                pathObjects.Clear();
                // todo - if(objectcollision)
            }
        }
        return pos;
    }

    public bool isLastStep()
    {
        return lastStep;
    }

    public void setLastStep(bool w)
    {
        lastStep = w;
    }


    public bool isWalking()
    {
        return walking;
    }

    public void setWalking(bool w)
    {
        walking = w;
    }
}