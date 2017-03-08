using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TownView;
using UI;
using UnityEngine.UI;


public class ExitButtonOnClick : MonoBehaviour  {

    GameObject cardWindow;
    GameObject exitButton;
    GameObject[] buildingObjects;

    public GameObject CardWindow
    {
        get
        {
            return cardWindow;
        }

        set
        {
            cardWindow = value;
        }
    }

    public GameObject ExitButton
    {
        get
        {
            return exitButton;
        }

        set
        {
            exitButton = value;
        }
    }

    public GameObject[] BuildingObjects
    {
        get
        {
            return buildingObjects;
        }

        set
        {
            buildingObjects = value;
        }
    }

    void Start()
    {

    }

    // Destroys all the given game objects and returns to the town screen
    void OnMouseDown()
    {
        for (int i = 0; i < BuildingObjects.Length; i++)
        {
            // TODO: make into list so we dont have to check for null?
            if (BuildingObjects[i] != null)
                BuildingObjects[i].GetComponent<PolygonCollider2D>().enabled = true;
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("toDestroy"))
            Destroy(go);

        // Redraw town

    }
}