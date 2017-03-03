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

    void OnMouseDown()
    {
        Debug.Log("DestroyWindow");
        Destroy(cardWindow);
        Destroy(exitButton);
        for (int i = 0; i < BuildingObjects.Length; i++)
        {
            BuildingObjects[i].GetComponent<PolygonCollider2D>().enabled = true;
        }
    }
}