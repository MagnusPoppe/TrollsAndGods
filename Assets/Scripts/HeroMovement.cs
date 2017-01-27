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
    public int heroSpeed;
    public int curSpeed;
    Vector2 curPos;
    Vector2 toPos;
    List<GameObject> pathObjects;
    bool pathMarked;
    int stepNumber;
    float animationSpeed;
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
            // When mousebutton is clicked, an already ongoing movement shall be stopped
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
            else if (gm.isNotBlocked((int)pos.x, (int)pos.y))
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
        else if(Input.GetKeyDown("space"))
        {
            if (IsWalking())
            {
                SetLastStep(true);
            }
            else if (pathMarked)
            {
                SetWalking(true);
            }
        }
        // Upon every update, hero will be moved in a direction if walking is enabled
        if (IsWalking())
        {
            Vector2 newPos = PrepareMovement();
            
            // If hero has reached a new tile, increment so that he walks towards the next one, reset time animation, and destroy tile object
            if (transform.position.Equals(pathObjects[stepNumber].transform.position))
            {
                Destroy(pathObjects[stepNumber]);
                stepNumber++;
                animationSpeed = 0f;
                // Stop the movement when amount of tiles moved has reached the limit, or walking is disabled
                if (IsLastStep())
                {
                    // Set hero position when he stops walking
                    curPos = transform.position;
                    SetWalking(false);
                    SetPathMarked(false);
                    RemoveMarkers(pathObjects);
                    // todo - if(objectcollision)
                }
            }
            // Execute the movement
            transform.position = newPos;
        }
    }

    /// <summary>
    /// Prepares movement variables, creates a list of positions and creates and returns a list of gameobjects
    /// </summary>
    /// <param name="pos">Destination tile position</param>
    /// <returns>List of instantiated marker objects</returns>
    private List<GameObject> MarkPath(Vector2 pos)
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
    /// Creates a position with animationspeed and returns it
    /// </summary>
    /// <returns>Position the hero shall be moved to</returns>
    private Vector2 PrepareMovement()
    {
        // Add animation, transform hero position
        animationSpeed += Time.deltaTime; 
        return Vector2.Lerp(transform.position, pathObjects[stepNumber].transform.position, animationSpeed);
    }

    /// <summary>
    /// Destroy the tile gameobjects and refresh list
    /// </summary>
    /// <param name="li">List that shall be cleared</param>
    private void RemoveMarkers(List<GameObject> li)
    {
        foreach (GameObject go in li)
            Destroy(go);
        li.Clear();
    }

    public bool IsLastStep()
    {
        return stepNumber == curSpeed || lastStep;
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
}