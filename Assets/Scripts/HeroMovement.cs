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
    List<GameObject> pathList = new List<GameObject>();
    List<Vector2> positions;
    bool pathMarked;
    int i;
    float move;
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
            else if (gm.canWalk[(int)pos.x, (int)pos.y])
            {
                // Walk to pointer if marked square is clicked by enabling variables that triggers moveHero method on update
                if (pathMarked && pos.Equals(toPos))
                {
                    prepareMovement();
                }
                // Activate clicked path
                else
                {
                    markPath(pos);
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
                prepareMovement();
            }
        }
        // Upon every update, it is checked if hero should be moved towards a destination
        if (isWalking())
        {
            moveHero();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos">Destination tile position</param>
    private void markPath(Vector2 pos)
    {
        pathMarked = true;
        toPos = pos;
        // Refresh already existing pointers
        foreach (GameObject go in pathList)
            Destroy(go);

        pathList.Clear();
        // Call algorithm method that returns a list of Vector2 positions to the point, go through all objects
        positions = aStar.calculate(curPos, pos);
        // Calculate how many steps the hero can move
        curSpeed = Math.Min(positions.Count, heroSpeed);
        // For each position, create a gameobject with an image and instantiate it, and add it to a gameobject list for later to be removed
        foreach (Vector2 no in positions)
        {
            // Create a cloned gameobject of a prefab, with the sprite according to what kind of a marker it is
            GameObject pathMarker;
            if (pos == no && curSpeed > 0)
                pathMarker = pathDestYes;
            else if (pos == no)
                pathMarker = pathDestNo;
            else if (curSpeed > 0)
                pathMarker = pathYes;
            else
                pathMarker = pathNo;
            curSpeed--;
            // set the cloned position to the vector2 object, instantiate it and add it to the list of gameobjects, pathList
            pathMarker.transform.position = no;
            pathMarker = Instantiate(pathMarker);
            pathList.Add(pathMarker);
        }
    }

    /// <summary>
    /// Sets variables so that movehero check in update is triggered
    /// </summary>
    private void prepareMovement()
    {
        curSpeed = Math.Min(positions.Count, heroSpeed);
        i = 0;
        move = 0f;
        setLastStep(false);
        setWalking(true);
    }

    /// <summary>
    /// Moves the object on the map
    /// </summary>
    private void moveHero()
    {
        // Add animation, transform hero position
        move += Time.deltaTime;
        transform.position = Vector2.Lerp(transform.position, positions[i], move);
        Vector2 pos = transform.position;

        // Every time the hero reaches a new tile, increment i so that he walks towards the next one, reset time animation, and destroy tile object
        if (pos.Equals(positions[i]))
        {
            // Destroy the tile object he has reached
            Destroy(pathList[i]);
            i++;
            move = 0f;
            // Stop the movement when amount of tiles moved has reached speed, or walking is disabled
            if (i == curSpeed || isLastStep())
            {
                stopMovement();
            }
        }
    }

    /// <summary>
    /// Sets variables so that movehero check in update is disabled
    /// </summary>
    private void stopMovement()
    {
        // Set hero position variable, and refresh toposition
        curPos = transform.position;
        toPos = new Vector2();
        setWalking(false);
        pathMarked = false;
        // Destroy the tile gameobjects
        foreach (GameObject go in pathList)
            Destroy(go);
        // todo - if(objectcollision)
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